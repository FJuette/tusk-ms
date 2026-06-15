## Context

The template is a `dotnet new` scaffold published as a NuGet package. Every generated project inherits these files verbatim, so bugs here multiply across all downstream projects. The issues identified range from silent data bugs (`new Guid()` sharing one in-memory database) to incorrect CQRS handler logic (request fields being ignored). None require new dependencies or structural redesign — all fixes are surgical, in-place corrections.

## Goals / Non-Goals

**Goals:**
- Correct the `new Guid()` → `Guid.NewGuid()` bug so each in-memory database context is isolated
- Make `CreateStoryCommand` handler use the `BusinessValue` and `Importance` fields from the request
- Remove the redundant `context.Attach(story)` in `ToggleDoneCommand`
- Fix the non-production error response in `CustomExceptionFilter` to serialize correctly
- Replace string literals in pipeline behaviors with `nameof()` / `GetType().Name`
- Fix CORS configuration so `AllowAnyOrigin()` doesn't silently override `WithOrigins()`
- Change the anonymous-user fallback in `GetClaimsFromUser` from `"Admin"` to an empty/neutral string so the query filter returns zero rows instead of admin-owned rows
- Pin `FluentValidation` in `Tusk.Application.csproj` to a stable release

**Non-Goals:**
- Adding new features to the template
- Changing the public HTTP API surface
- Modifying test structure beyond what is needed for the Guid fix

## Decisions

### D1: `Guid.NewGuid()` for in-memory database isolation

Current code: `optionsBuilder.UseInMemoryDatabase(new Guid().ToString())` — `new Guid()` always returns `00000000-0000-0000-0000-000000000000`. Every non-production `TuskDbContext` shares one database, so parallel tests or multiple contexts in a single request see each other's data.

**Decision**: Replace with `Guid.NewGuid().ToString()`.

Alternatives considered: using a fixed test-database name and relying on `EnsureDeleted` in tests — this is already done in `UserStoryTests.Seed()` but is fragile when multiple contexts exist simultaneously.

### D2: `CreateStoryCommand` must use request values

The handler creates `UserStory` with `Priority.Create(1).Value` (hardcoded) and `BusinessValue.BV1000` (hardcoded) regardless of what the caller sent. The validator already enforces valid `BusinessValue` ids, so the handler can safely look up the entity.

**Decision**: Look up `BusinessValue` from context using `request.BusinessValue` as the id. Pass `request.Importance` to the `UserStory` constructor.

Alternatives considered: returning a 400 if the `BusinessValue` id is not found — the validator already guards this, so a `NotFoundException` is an acceptable fallback; no extra guard needed.

### D3: Remove `context.Attach` in `ToggleDoneCommand`

The story is loaded by `SingleOrDefaultAsync` on the same `context` instance. EF Core already tracks it. Calling `context.Attach(story)` on a tracked entity with `EntityState.Modified` (or any state) throws `InvalidOperationException` in EF Core 7+.

**Decision**: Delete the `context.Attach(story)` call; `SaveChangesAsync` will persist the toggle via change tracking.

### D4: Fix `CustomExceptionFilter` dev error response

Non-production path: `new JsonResult(new { error = returnMessage, stackTrace = ... })` — `returnMessage` is already a `JsonResult` object (not its value), so serialization produces `{ "error": { "Value": {...}, "StatusCode": null, ... }, ... }`.

**Decision**: Extract the value from `returnMessage` using its `.Value` property before embedding it in the outer object.

### D5: `nameof()` in pipeline behaviors

String literals like `"EventLoggerBehavior"` break silently when the class is renamed.

**Decision**: Replace with `nameof(EventLoggerBehavior)` / `nameof(RequestPerformanceBehavior)` inside the respective classes, or use `GetType().Name` for a more idiomatic pattern.

### D6: CORS — separate dev and prod origins

`AllowAnyOrigin()` combined with `WithOrigins(...)` in the same policy renders `WithOrigins` a no-op. A template should demonstrate the intentional choice explicitly.

**Decision**: Keep `AllowAnyOrigin()` only if explicitly in development; wrap it in an `if (env.IsDevelopment())` branch. The `TODO remove in production` comment becomes code.

Note: `AllowAnyOrigin()` and `AllowCredentials()` cannot be combined per CORS spec — the existing policy is already credentials-free, so no secondary breakage.

### D7: Anonymous-caller fallback in `GetClaimsFromUser`

Falling back to `"Admin"` means an unauthenticated request silently passes the `OwnedBy == _userId` query filter and sees admin-owned data. When `DisableAuthentication` is false, the auth middleware should prevent unauthenticated calls from reaching the controller, but the DbContext itself should not trust that assumption.

**Decision**: Change the fallback to `string.Empty`. The query filter `x.OwnedBy == ""` returns zero rows for unrecognized callers, which is the safe default.

### D8: Pin `FluentValidation` to stable

`Tusk.Application.csproj` references `FluentValidation 12.0.0-preview1`. Preview packages are unsuitable for a published template.

**Decision**: Downgrade to the latest stable `11.x` release that is compatible with `FluentValidation.AspNetCore 11.3.0` referenced in `Tusk.Api.csproj`.

## Risks / Trade-offs

- **D2 (BusinessValue lookup)**: Adds one DB roundtrip per create. Acceptable for a template that demonstrates patterns, not benchmarks. → No mitigation needed.
- **D7 (empty fallback)**: If `DisableAuthentication true` is used, the empty-string filter breaks all reads (no stories will ever be owned by `""`). → The seeder and test context must set `OwnedBy` explicitly; tests already do via `TuskDbContext(DbContextOptions)` which uses `"Tester"`. The `DisableAuthentication` path should set a well-known identity string (e.g., `"anonymous"`) in `GetClaimsFromUser` rather than empty string. Document this in a comment.

## Open Questions

- Should `DisableAuthentication true` skip the `GetClaimsFromUser` registration entirely and inject a fixed `SystemUser` claims provider instead? (out of scope for this change — document as a TODO)

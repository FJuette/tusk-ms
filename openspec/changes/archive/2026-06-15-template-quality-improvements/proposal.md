## Why

The template contains several bugs and quality issues that will propagate to every project generated from it, giving developers a broken or misleading starting point. Fixing these now prevents the same mistakes from being copy-pasted into production codebases.

## What Changes

- **BUG FIX**: `TuskDbContext` uses `new Guid()` (always the empty GUID `00000000-...`) instead of `Guid.NewGuid()` for the in-memory database name — all non-production contexts currently share one database
- **BUG FIX**: `CreateStoryCommand` handler ignores the `BusinessValue` and `Importance` fields from the request and hardcodes `Priority.Create(1).Value` and `BusinessValue.BV1000` instead
- **BUG FIX**: `CustomExceptionFilter` non-production error response wraps a `JsonResult` object in a second object, producing a broken/unreadable error payload
- **BUG FIX**: `ToggleDoneCommand` calls `context.Attach(story)` on a story already tracked by the same context (loaded via `SingleOrDefaultAsync`), risking an `InvalidOperationException`
- **QUALITY**: `EventLoggerBehavior` and `RequestPerformanceBehavior` use string literals (`"EventLoggerBehavior"`) instead of `nameof()` — breaks silently on rename
- **QUALITY**: `FluentValidation.AspNetCore` 11.3.0 (Api) references `FluentValidation` 12.0.0-preview1 (Application) — cross-version dependency using a pre-release package
- **QUALITY**: CORS policy calls both `WithOrigins("http://localhost:4200")` and `AllowAnyOrigin()` — the specific origin is a dead letter, every origin is allowed
- **QUALITY**: `GetClaimsFromUser` falls back to `"Admin"` as the user ID for unauthenticated requests, meaning the row-level security filter leaks admin-owned data to anonymous callers
- **QUALITY**: `StoryController` carries no `[Authorize]` attribute — the template doesn't demonstrate where to place auth, and disabling JWT auth becomes invisible to readers

## Capabilities

### New Capabilities

- `cors-config`: Demonstrate a correct, intention-revealing CORS setup that separates dev/prod origins without contradictory directives

### Modified Capabilities

- `story-commands`: Fix CreateStory to use request values; fix ToggleDone to not double-attach
- `story-ownership`: Fix anonymous-caller identity fallback so it does not default to "Admin"
- `exception-handling`: Fix non-production error response serialization
- `pipeline-behaviors`: Replace string literals with `nameof()` / `GetType().Name`

## Impact

- `template/src/Tusk.Api/Persistence/TuskDbContext.cs` — Guid fix
- `template/src/Tusk.Application/Stories/Commands/CreateStoryCommand.cs` — use request fields
- `template/src/Tusk.Application/Stories/Commands/ToggleDoneCommand.cs` — remove redundant Attach
- `template/src/Tusk.Api/Filters/CustomExceptionFilter.cs` — fix dev error response
- `template/src/Tusk.Application/Behaviours/EventLoggerBehavior.cs` — nameof
- `template/src/Tusk.Application/Behaviours/RequestPerformanceBehavior.cs` — nameof
- `template/src/Tusk.Api/Startup.cs` — CORS + [Authorize] guidance comment
- `template/src/Tusk.Api/Infrastructure/IGetClaimsProvider.cs` — safe anonymous fallback
- `template/src/Tusk.Application/Tusk.Application.csproj` — pin FluentValidation to stable release
- No public API surface changes; no breaking changes for template consumers

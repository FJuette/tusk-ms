## Context

The template currently uses AutoMapper 12.x for entity-to-DTO projection in two query handlers (`GetStoryQuery`, `GetAllStoriesQuery`). AutoMapper requires a `Profile` subclass per mapping, an `IMapper` injected into handlers, and `.ProjectTo<T>(mapper.ConfigurationProvider)` for LINQ-level projection. The test layer rebuilds a `Mapper` instance from profiles via `FakeFactory.GetMapper`.

Mapster offers an equivalent API surface with lower runtime cost: no reflection-heavy configuration bootstrap, source-gen adapters (optional), and a compatible `.ProjectToType<T>()` queryable extension.

## Goals / Non-Goals

**Goals:**
- Remove all AutoMapper packages and replace with Mapster equivalents
- Preserve identical mapping semantics (same DTO shape, same LINQ-level projection)
- Keep the mapping config co-located with the query handler (same file pattern)
- Tests continue to verify mapping correctness without framework-specific boilerplate

**Non-Goals:**
- Adopting Mapster source generators (requires SDK tooling changes; can be done later)
- Changing DTO shapes or adding new projections
- Migrating mappings in tests to use `.Adapt<T>()` extension methods (keeping `IMapper` for test compatibility)

## Decisions

### D1 — Use `IRegister` for per-handler mapping configuration

**Decision:** Replace each `Profile` class with a class implementing `TypeAdapterConfig`-based registration via Mapster's `IRegister` interface.

**Rationale:** `IRegister` is Mapster's direct equivalent of `Profile`. It keeps mapping config in the same file as the handler (matching the existing co-location pattern) and is picked up automatically by `services.AddMapster()` assembly scan.

**Alternative considered:** Inline `.Map()` fluent calls on `TypeAdapterConfig.GlobalSettings` at startup — rejected because it breaks co-location and makes config order-sensitive.

### D2 — Use `.ProjectToType<T>()` for queryable projection

**Decision:** Replace `.ProjectTo<T>(mapper.ConfigurationProvider)` with Mapster's `.ProjectToType<T>()`.

**Rationale:** `ProjectToType<T>` is a drop-in replacement that reads from `TypeAdapterConfig.GlobalSettings` (or a passed config). No mapper instance needed at call site, simplifying handler signatures.

**Alternative considered:** Keep `IMapper` injection and call `.Map<List<T>>()` after `.ToListAsync()` — rejected because it pulls the full entity graph into memory before mapping, losing the SQL projection benefit.

### D3 — Replace `IMapper` injection with `TypeAdapterConfig` in handlers

**Decision:** Handlers that previously injected `IMapper` will instead inject `TypeAdapterConfig` (or nothing, relying on global config).

**Rationale:** `ProjectToType<T>()` does not require a mapper instance; removing it from handler constructors simplifies the DI graph.

**Alternative considered:** Keep `IMapper` via Mapster's `MapsterMapper.IMapper` adapter — rejected as unnecessary indirection when handlers only project queryables.

### D4 — Test mapper via `TypeAdapterConfig` directly

**Decision:** Replace `FakeFactory.GetMapper(profiles)` with a helper that builds a scoped `TypeAdapterConfig`, applies the relevant `IRegister` classes, and returns it.

**Rationale:** Tests need isolated configs to avoid cross-test pollution. Mapster's `TypeAdapterConfig` is clonable and supports scoped instances.

## Risks / Trade-offs

- [Risk] Mapster's `ProjectToType<T>()` behavior may differ from AutoMapper's `ProjectTo<T>()` for edge cases (nulls in nested collections) → **Mitigation**: Existing integration tests cover the affected mappings; run full test suite to confirm parity.
- [Risk] `TypeAdapterConfig.GlobalSettings` is a static singleton; parallel tests using global config could interfere → **Mitigation**: Test helper creates isolated `TypeAdapterConfig` instances per test, not the global one.
- [Trade-off] Removing `IMapper` from handler constructors is a small but visible API change for anyone who has already generated a project from this template → Acceptable; this is a template, not a shipped library.

## Migration Plan

1. Swap NuGet packages in both `.csproj` files
2. Update `Startup.cs` DI registration
3. Replace `Profile` → `IRegister` in both query files, update handlers to use `.ProjectToType<T>()`
4. Update `FakeFactory` and test constructors
5. Run `dotnet test` — all tests must pass before merging

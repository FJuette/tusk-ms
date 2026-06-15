### Requirement: Entity-to-DTO mapping via Mapster type adapters
The system SHALL use Mapster `IRegister` implementations to define object-to-object mappings between domain entities and DTOs. Each mapping configuration SHALL be co-located in the same file as the query handler that uses it.

#### Scenario: Mapping config registered at startup
- **WHEN** the application starts
- **THEN** all `IRegister` implementations in `Tusk.Application` SHALL be discovered and applied to the global `TypeAdapterConfig` via `services.AddMapster()`

#### Scenario: Custom member mapping respected
- **WHEN** a DTO property name differs from the source entity property (e.g., `Priority.Value` → `Priority`)
- **THEN** the `IRegister` SHALL define an explicit `.Map()` rule and the resulting DTO SHALL contain the correct projected value

### Requirement: Queryable LINQ projection using Mapster
Query handlers that project entity queryables to DTOs SHALL use Mapster's `.ProjectToType<T>()` extension method so that projection is translated to SQL rather than executed in memory.

#### Scenario: GetAllStories projects at database level
- **WHEN** `GetAllStoriesQueryHandler` is invoked
- **THEN** the EF Core queryable SHALL be projected to `UserStoriesDto` via `.ProjectToType<UserStoriesDto>()` before `ToListAsync()` is called

#### Scenario: GetStory projects at database level
- **WHEN** `GetStoryQueryHandler` is invoked with a valid story ID
- **THEN** the EF Core queryable SHALL be projected to `UserStoryDto` via `.ProjectToType<UserStoryDto>()` before `FirstOrDefaultAsync()` is called

### Requirement: Isolated mapping config in tests
Tests that verify mapping behavior SHALL use a scoped `TypeAdapterConfig` instance (not the global singleton) to avoid cross-test interference.

#### Scenario: Test mapping config is isolated
- **WHEN** a test constructs a query handler with a specific mapping config
- **THEN** that config SHALL NOT affect mappings in other tests running in the same process

#### Scenario: Test helper constructs scoped mapper
- **WHEN** `FakeFactory` is called to create a mapper for tests
- **THEN** it SHALL return a `MapsterMapper.Mapper` instance backed by a new `TypeAdapterConfig` with the supplied `IRegister` classes applied

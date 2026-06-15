## ADDED Requirements

### Requirement: CreateStory uses request values for BusinessValue and Importance
The `CreateStoryCommand` handler SHALL create the `UserStory` using the `BusinessValue` id and `Importance` value supplied in the request, not hardcoded defaults.

#### Scenario: BusinessValue from request is persisted
- **WHEN** `CreateStoryCommand` is handled with `BusinessValue = 2`
- **THEN** the created `UserStory.BusinessValue` SHALL have id `2`

#### Scenario: Importance from request is persisted
- **WHEN** `CreateStoryCommand` is handled with `Importance = MustHave`
- **THEN** the created `UserStory.Importance` SHALL equal `UserStory.Relevance.MustHave`

### Requirement: In-memory database contexts are isolated per instance
Each `TuskDbContext` created without explicit `DbContextOptions` (i.e., using the env-based constructor in non-production) SHALL receive a unique in-memory database name so that no two context instances share data unintentionally.

#### Scenario: Two non-production contexts are independent
- **WHEN** two `TuskDbContext` instances are created independently in a non-production environment
- **THEN** data written through one context SHALL NOT be readable through the other context

### Requirement: ToggleDone does not re-attach a tracked entity
The `ToggleDoneCommand` handler SHALL NOT call `context.Attach(story)` after loading the story via `SingleOrDefaultAsync` on the same context, as the entity is already tracked.

#### Scenario: Toggle completes without InvalidOperationException
- **WHEN** `ToggleDoneCommand` is handled for a valid story and task
- **THEN** the task's done state SHALL be toggled and persisted without throwing `InvalidOperationException`

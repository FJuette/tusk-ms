### Requirement: Request dispatching via DispatchR
The system SHALL dispatch commands and queries using the DispatchR dispatcher. All types that implement the request marker interface SHALL be routable to exactly one handler.

#### Scenario: Command is dispatched and handled
- **WHEN** a controller sends a command via the DispatchR dispatcher
- **THEN** the corresponding command handler executes and returns its result

#### Scenario: Query is dispatched and handled
- **WHEN** a controller sends a query via the DispatchR dispatcher
- **THEN** the corresponding query handler executes and returns its result

#### Scenario: No handler registered
- **WHEN** a request type has no registered handler
- **THEN** the dispatcher SHALL throw an exception at runtime (not silently no-op)

### Requirement: Notification publishing via DispatchR
The system SHALL publish notifications (events) using the DispatchR notification mechanism. Zero or more handlers MAY be registered per notification type.

#### Scenario: Notification is published with one handler
- **WHEN** a handler publishes a notification that has one registered notification handler
- **THEN** that notification handler executes

#### Scenario: Notification is published with no handlers
- **WHEN** a handler publishes a notification that has no registered notification handlers
- **THEN** the publish call completes without error

### Requirement: Pipeline behaviors execute for every request
The system SHALL execute registered pipeline behaviors in order for every dispatched request. Behaviors SHALL wrap handler execution (before/after semantics).

#### Scenario: EventLoggerBehavior executes
- **WHEN** any request is dispatched
- **THEN** EventLoggerBehavior logs before and after handler execution

#### Scenario: RequestPerformanceBehavior executes
- **WHEN** any request is dispatched
- **THEN** RequestPerformanceBehavior measures elapsed time and logs a warning if execution exceeds 5 seconds

### Requirement: Zero MediatR references at compile time
After migration the solution SHALL contain no references to the MediatR NuGet package. The project SHALL compile cleanly with `dotnet build`.

#### Scenario: Build succeeds without MediatR
- **WHEN** `dotnet build` is run on the solution
- **THEN** the build succeeds and no MediatR-related namespaces or types are referenced

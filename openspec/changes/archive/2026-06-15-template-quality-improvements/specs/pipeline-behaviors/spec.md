## ADDED Requirements

### Requirement: Pipeline behavior log entries identify the behavior by compile-safe name
`EventLoggerBehavior` and `RequestPerformanceBehavior` SHALL identify themselves in log entries using a compile-safe expression (`nameof()` or `GetType().Name`) rather than a string literal, so that renames are caught at compile time.

#### Scenario: EventLoggerBehavior log entry uses correct name
- **WHEN** any request is dispatched and `EventLoggerBehavior` executes
- **THEN** the log entry SHALL contain the string `"EventLoggerBehavior"` derived from a compile-safe source (not a hardcoded literal)

#### Scenario: RequestPerformanceBehavior log entry uses correct name
- **WHEN** any request is dispatched and `RequestPerformanceBehavior` executes
- **THEN** the log entry SHALL contain the string `"RequestPerformanceBehavior"` derived from a compile-safe source

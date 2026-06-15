### Requirement: CORS policy does not silently allow all origins in production
The CORS policy SHALL NOT call `AllowAnyOrigin()` unconditionally. In non-development environments only the explicitly listed origins SHALL be permitted.

#### Scenario: Development environment allows all origins
- **WHEN** the application runs in the Development environment
- **THEN** the CORS policy SHALL permit requests from any origin (for DX convenience)

#### Scenario: Non-development environment restricts origins
- **WHEN** the application runs in any environment other than Development
- **THEN** the CORS policy SHALL only permit requests from explicitly configured origins and SHALL NOT call `AllowAnyOrigin()`

#### Scenario: WithOrigins is not overridden
- **WHEN** a CORS policy calls both `WithOrigins(...)` and `AllowAnyOrigin()` for the same named policy
- **THEN** the template SHALL NOT produce such a configuration — the two directives SHALL NOT coexist in the same policy branch

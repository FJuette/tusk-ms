### Requirement: Non-production error responses serialize the error payload, not a JsonResult wrapper
In non-production environments, `CustomExceptionFilter` SHALL include the error content (dictionary or message) and the stack trace in the HTTP response body as a flat JSON object. It SHALL NOT serialize a `JsonResult` C# object as the `error` field.

#### Scenario: ValidationException error response in development
- **WHEN** a `ValidationException` is thrown and the environment is not Production
- **THEN** the response body SHALL contain the validation error dictionary under an `error` key and the stack trace under a `stackTrace` key, both serialized as plain JSON values

#### Scenario: Generic exception error response in development
- **WHEN** an unhandled exception is thrown and the environment is not Production
- **THEN** the response body SHALL contain the exception message string under an `error` key and the stack trace under a `stackTrace` key

#### Scenario: Production response omits stack trace
- **WHEN** any exception is thrown and the environment is Production
- **THEN** the response body SHALL contain only the error payload (no `stackTrace` field)

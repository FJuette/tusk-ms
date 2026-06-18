### Requirement: OpenAPI document served via built-in ASP.NET Core OpenAPI
The API SHALL expose its OpenAPI document at `/openapi/v1.json` using `Microsoft.AspNetCore.OpenApi` (`MapOpenApi()`). Swashbuckle SHALL NOT be used.

#### Scenario: OpenAPI document accessible
- **WHEN** a GET request is made to `/openapi/v1.json`
- **THEN** the response SHALL be HTTP 200 with `Content-Type: application/json` containing a valid OpenAPI 3.x document

### Requirement: Scalar UI serves interactive documentation
The API SHALL mount Scalar UI at `/scalar/v1` backed by the `/openapi/v1.json` document, providing an interactive interface equivalent to Swagger UI.

#### Scenario: Scalar UI accessible in development
- **WHEN** the application is running and a browser navigates to `/scalar/v1`
- **THEN** the Scalar UI SHALL load and display the API's operations

### Requirement: JWT Bearer security scheme declared in the OpenAPI document
When authentication is enabled (i.e., `DisableAuthentication` is `false`), the OpenAPI document SHALL declare a `Bearer` security scheme and apply it globally, so that the interactive UI presents an "Authorize" flow.

#### Scenario: Bearer scheme present in OpenAPI document
- **WHEN** authentication is enabled and the OpenAPI document is fetched from `/openapi/v1.json`
- **THEN** the `components.securitySchemes` object SHALL contain a `Bearer` entry with `type: http` and `scheme: bearer`
- **THEN** the top-level `security` array SHALL reference the `Bearer` scheme

#### Scenario: No security scheme when authentication disabled
- **WHEN** the template is generated with `--DisableAuthentication true` and the OpenAPI document is fetched
- **THEN** the `components.securitySchemes` object SHALL NOT contain a `Bearer` entry

## Why

The template still carries .NET 5–8 era patterns that are now considered legacy in a .NET 10 world: Swashbuckle for OpenAPI docs (unsupported by Microsoft for .NET 9+), Newtonsoft.Json as the serializer (a heavyweight dependency replaced by the built-in STJ), and the old `UseEndpoints` routing call. Refactoring these aligns the template with the idioms developers expect to see in a brand-new .NET 10 project.

## What Changes

- **BREAKING** Replace `Swashbuckle.AspNetCore` + `Swashbuckle.AspNetCore.Newtonsoft` with `Microsoft.AspNetCore.OpenApi` (already a dependency) + Scalar UI for interactive docs
- **BREAKING** Remove `Microsoft.AspNetCore.Mvc.NewtonsoftJson` and `Newtonsoft.Json`; migrate to `System.Text.Json` with reference-cycle handling (`ReferenceHandler.IgnoreCycles`)
- **BREAKING** Replace the legacy `app.UseEndpoints(e => e.MapControllers())` call with the modern `app.MapControllers()` directly on `WebApplication`
- Replace `JObject`/`JProperty` health-check writer (Newtonsoft) with `JsonSerializer` (STJ)
- Remove `services.AddEndpointsApiExplorer()` (already included in `MapOpenApi()`)

## Capabilities

### New Capabilities

- `openapi-documentation`: OpenAPI spec served via `MapOpenApi()` and Scalar UI at `/scalar/v1`; JWT Bearer security scheme wired via `AddOpenApi()` document transformer

### Modified Capabilities

- `exception-handling`: Non-production error response writer migrated from Newtonsoft `JObject` to `System.Text.Json.JsonSerializer`

## Impact

- `template/src/Tusk.Api/Tusk.Api.csproj` — remove Swashbuckle packages and `Newtonsoft.Json` / `NewtonsoftJson` packages; add `Scalar.AspNetCore`
- `template/src/Tusk.Api/Infrastructure/SwaggerServiceExtensions.cs` — rewrite or replace with `OpenApiServiceExtensions.cs` using the built-in OpenAPI API
- `template/src/Tusk.Api/Startup.cs` — update `ConfigureServices` and `Configure`; switch serializer and routing call
- `template/src/Tusk.Api/Filters/CustomExceptionFilter.cs` — replace Newtonsoft serialization with STJ
- No changes to domain, application, or test projects
- No public API surface changes for template consumers; generated app behaviour identical from an HTTP perspective

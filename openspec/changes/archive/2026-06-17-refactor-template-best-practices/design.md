## Context

The template was originally written targeting .NET 9 and brought along two heavyweight third-party packages that are now considered legacy: Swashbuckle for OpenAPI UI (Microsoft stopped publishing guidance for Swashbuckle with .NET 9+) and Newtonsoft.Json as the JSON serializer (superseded by the built-in `System.Text.Json` in all new project templates since .NET 5). Additionally, the pipeline still uses the old `UseEndpoints` wrapper that was replaced by first-class extension methods on `WebApplication` in .NET 6.

## Goals / Non-Goals

**Goals:**
- Serve OpenAPI docs via `Microsoft.AspNetCore.OpenApi` (already a dependency) and Scalar UI
- Remove Swashbuckle and Newtonsoft.Json from the dependency tree
- Migrate the JSON serializer to `System.Text.Json` with equivalent behaviour
- Use `app.MapControllers()` directly instead of `app.UseEndpoints`

**Non-Goals:**
- Migrating from `Startup.cs` to a single top-level `Program.cs` (out of scope; `Startup.cs` aids readability in a teaching template)
- Changing any domain, application, or test project code
- Altering any HTTP contracts (status codes, field names, auth flow)

## Decisions

### 1. OpenAPI: `MapOpenApi()` + Scalar UI

**Decision**: Replace `AddSwaggerGen` / `UseSwagger` / `UseSwaggerUI` with `builder.Services.AddOpenApi()` + `app.MapOpenApi()` + `app.MapScalarApiReference()`.

**Rationale**: `Microsoft.AspNetCore.OpenApi` is the first-party solution from the ASP.NET Core team for .NET 9/10. Swashbuckle is no longer actively maintained for these versions and requires the `Newtonsoft` compatibility shim. Scalar provides a polished interactive UI equivalent to Swagger UI with a simpler setup.

**JWT Bearer security**: The built-in OpenAPI API exposes an `IOpenApiDocumentTransformer` seam. A small document transformer injects the `securitySchemes` and global `security` entries, replacing the `AddSecurityDefinition` / `AddSecurityRequirement` calls.

**Alternatives considered:**
- Keep Swashbuckle 10.x — already present but adds two packages and requires Newtonsoft shim; not the direction Microsoft recommends.
- Use Swagger UI with `Microsoft.AspNetCore.OpenApi` — possible, but requires manually serving the Swagger UI bundle; Scalar is the canonical pairing.

### 2. Serializer: `System.Text.Json` (STJ)

**Decision**: Remove `AddNewtonsoftJson()` and the Newtonsoft.Json / `Swashbuckle.AspNetCore.Newtonsoft` packages. Configure STJ with `ReferenceHandler.IgnoreCycles` to preserve the reference-loop-handling behaviour.

**Rationale**: STJ is the default for all new ASP.NET Core projects. It is faster, allocation-efficient, and AOT-compatible. The only behaviour gap relevant here is reference-cycle handling, which STJ supports natively via `JsonSerializerOptions`.

**Health-check writer**: The `WriteHealthCheckResponse` helper currently builds a `JObject` tree with Newtonsoft. This is replaced with an equivalent anonymous-object serialized via `JsonSerializer.Serialize` (STJ).

**Alternatives considered:**
- Keep Newtonsoft.Json — contradicts the goal; leaves a >1 MB dependency in the template.

### 3. Pipeline: `app.MapControllers()` directly

**Decision**: Replace `app.UseEndpoints(endpoints => endpoints.MapControllers())` with `app.MapControllers()`.

**Rationale**: `MapControllers()` became a direct extension on `IEndpointRouteBuilder` (and thus on `WebApplication`) in .NET 6. The `UseEndpoints` wrapper is a legacy shim that still works but is unnecessary and visually noisy.

## Risks / Trade-offs

- **Scalar UI not first-party** → Mitigation: `Scalar.AspNetCore` is widely adopted; the package is MIT-licensed and actively maintained. Worst case: swap for Swagger UI bundle with no API changes.
- **STJ enum serialisation** → By default STJ serialises enums as integers. The template's current contracts only expose primitive fields so this is not a breaking change, but generated-project developers must be aware.
- **`MapOpenApi()` requires `AddEndpointsApiExplorer`** → This is included automatically by the built-in OpenAPI setup; the explicit call in `SwaggerServiceExtensions` can be removed.

## Migration Plan

1. Update `Tusk.Api.csproj`: remove Swashbuckle + Newtonsoft packages, add `Scalar.AspNetCore`.
2. Rewrite `SwaggerServiceExtensions.cs` → `OpenApiServiceExtensions.cs` with `AddOpenApi()` + JWT document transformer + Scalar mapping.
3. Update `Startup.cs`: replace `AddSwaggerDocumentation()` / `UseSwaggerDocumentation()` calls; replace `AddNewtonsoftJson()` with STJ options; replace `UseEndpoints`.
4. Update `CustomExceptionFilter.cs`: remove Newtonsoft `using`; no functional change needed (uses `JsonResult` which delegates to the configured serializer).
5. Update `Startup.cs` health-check writer: replace `JObject`/`JProperty` with STJ `JsonSerializer.Serialize`.
6. Build and test.

Rollback: revert the `.csproj` and affected source files — no database or config changes.

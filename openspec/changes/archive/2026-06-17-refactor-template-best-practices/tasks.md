## 1. Update Project Dependencies

- [x] 1.1 Remove `Swashbuckle.AspNetCore`, `Swashbuckle.AspNetCore.Newtonsoft`, `Microsoft.AspNetCore.Mvc.NewtonsoftJson`, and `Newtonsoft.Json` from `template/src/Tusk.Api/Tusk.Api.csproj`
- [x] 1.2 Add `Scalar.AspNetCore` to `template/src/Tusk.Api/Tusk.Api.csproj`
- [x] 1.3 Run `dotnet restore` from `template/` and confirm it succeeds with no unresolved packages

## 2. Replace OpenAPI Infrastructure

- [x] 2.1 Delete `template/src/Tusk.Api/Infrastructure/SwaggerServiceExtensions.cs`
- [x] 2.2 Create `template/src/Tusk.Api/Infrastructure/OpenApiServiceExtensions.cs` with `AddApiDocumentation()` calling `services.AddOpenApi()` with a JWT Bearer document transformer (wrapped in `#if (!DisableAuthentication)`)
- [x] 2.3 Add `UseApiDocumentation()` extension method that calls `app.MapOpenApi()` and `app.MapScalarApiReference()`

## 3. Update Startup.cs

- [x] 3.1 Replace `services.AddSwaggerDocumentation()` with `services.AddApiDocumentation()` in `ConfigureServices`
- [x] 3.2 Replace `services.AddControllers(...).AddNewtonsoftJson(...)` with `services.AddControllers(...)` + `services.ConfigureHttpJsonOptions(o => o.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)`
- [x] 3.3 Replace `app.UseSwaggerDocumentation()` with `app.UseApiDocumentation()` in `Configure`
- [x] 3.4 Replace `app.UseEndpoints(endpoints => endpoints.MapControllers())` with `app.MapControllers()`
- [x] 3.5 Remove `using Newtonsoft.Json;` and `using Newtonsoft.Json.Linq;` from `Startup.cs`
- [x] 3.6 Rewrite `WriteHealthCheckResponse` to use `System.Text.Json.JsonSerializer.Serialize` instead of `JObject`/`JProperty`

## 4. Verify

- [x] 4.1 Run `dotnet build` from `template/` and confirm zero errors
- [x] 4.2 Run `dotnet test` from `template/` and confirm all tests pass
- [x] 4.3 Start the app with `dotnet run` and confirm Scalar UI loads at `/scalar/v1`
- [x] 4.4 Confirm `/openapi/v1.json` returns a valid OpenAPI document with `Bearer` security scheme present
- [x] 4.5 Confirm `/api/health` returns a well-formed JSON health report

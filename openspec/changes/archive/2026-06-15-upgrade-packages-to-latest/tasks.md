## 1. Upgrade Target Framework

- [x] 1.1 Update `<TargetFramework>` from `net9.0` to `net10.0` in `template/src/Tusk.Api/Tusk.Api.csproj`
- [x] 1.2 Update `<TargetFramework>` from `net9.0` to `net10.0` in `template/src/Tusk.Application/Tusk.Application.csproj`
- [x] 1.3 Update `<TargetFramework>` from `net9.0` to `net10.0` in `template/src/Tusk.Domain/Tusk.Domain.csproj`
- [x] 1.4 Update `<TargetFramework>` from `net9.0` to `net10.0` in `template/tests/Tusk.Api.Tests/Tusk.Api.Tests.csproj`

## 2. Update Package Versions

- [x] 2.1 Update `Tusk.Api` packages: `Microsoft.AspNetCore.OpenApi` → 10.0.9, `AspNetCore.HealthChecks.SqlServer` → latest, `Mapster` → 10.0.8, `FluentValidation.AspNetCore` → 11.3.1, `Microsoft.AspNetCore.Authentication.JwtBearer` → 10.0.9, `Microsoft.AspNetCore.Mvc.NewtonsoftJson` → 10.0.9, `Microsoft.CodeAnalysis.NetAnalyzers` → 10.0.301, `Microsoft.EntityFrameworkCore.InMemory` → 10.0.9, `Newtonsoft.Json` → 13.0.4, `Serilog.AspNetCore` → 10.0.0, `Serilog.Sinks.Console` → 6.1.1, `Swashbuckle.AspNetCore` → 10.2.1, `Microsoft.EntityFrameworkCore.Design` → 10.0.9, `Microsoft.EntityFrameworkCore.SqlServer` → 10.0.9, `Swashbuckle.AspNetCore.Newtonsoft` → 10.2.1
- [x] 2.2 Update `Tusk.Application` packages: `Mapster` → 10.0.8, `FluentValidation` → 12.1.1, `Microsoft.EntityFrameworkCore` → 10.0.9, `Serilog` → 4.3.1
- [x] 2.3 Update `Tusk.Domain` packages: `CSharpFunctionalExtensions` → 3.7.0
- [x] 2.4 Update `Tusk.Api.Tests` packages: `FluentAssertions` → 8.10.0, `Microsoft.NET.Test.Sdk` → 18.6.0, `coverlet.collector` → 10.0.1, `xunit.runner.visualstudio` → 3.1.5

## 3. Fix FluentValidation 12 Breaking Changes

- [x] 3.1 Replace any `AddFluentValidation()` registration in `Startup.cs` with `AddValidatorsFromAssemblyContaining<...>()` or equivalent supported DI registration
- [x] 3.2 Verify FluentValidation pipeline behavior (`ValidationBehavior`) still compiles and functions correctly with v12 interfaces

## 4. Fix Mapster 10 Breaking Changes

- [x] 4.1 Verify `TypeAdapterConfig` registration in `Startup.cs` compiles with Mapster 10
- [x] 4.2 Verify mapping calls in query handlers compile and produce correct results with Mapster 10

## 5. Fix Swashbuckle 10 Breaking Changes

- [x] 5.1 Update `UseSwagger` / `UseSwaggerUI` / `AddSwaggerGen` calls in `Startup.cs` to the Swashbuckle 10.x API (or replace with `MapOpenApi()` + Swagger UI if Swashbuckle is no longer needed)
- [x] 5.2 Verify Swagger UI is accessible at `/swagger` after changes

## 6. Update Dockerfile

- [x] 6.1 Replace `mcr.microsoft.com/dotnet/sdk:9.0` build image with `mcr.microsoft.com/dotnet/sdk:10.0` in Dockerfile
- [x] 6.2 Replace `mcr.microsoft.com/dotnet/aspnet:9.0` runtime image with `mcr.microsoft.com/dotnet/aspnet:10.0` in Dockerfile

## 7. Verify

- [x] 7.1 Run `dotnet build` from `template/` and confirm zero errors
- [x] 7.2 Run `dotnet test` from `template/` and confirm all tests pass
- [x] 7.3 Run `dotnet list package --outdated` and confirm no packages are reported as outdated

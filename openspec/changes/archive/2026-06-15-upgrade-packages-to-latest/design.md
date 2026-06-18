## Context

The template currently targets `net9.0` and pins packages from the .NET 9 / mid-2024 era. Running `dotnet list package --outdated` reveals 18 outdated packages across four projects, including several with major version bumps that bundle breaking changes. Because this is a code-generator template (not a running service), upgrades take effect in every project a developer bootstraps from it going forward — there is no live migration of existing data or runtime state.

## Goals / Non-Goals

**Goals:**
- Upgrade target framework from `net9.0` to `net10.0` across all projects
- Update every NuGet package to its latest stable release
- Fix any compilation or runtime breakage introduced by major-version library changes
- Keep the template building (`dotnet build`) and all tests passing (`dotnet test`)

**Non-Goals:**
- Adopting new .NET 10 / library features beyond what is needed to compile
- Refactoring existing code to use new APIs when old ones still work
- Upgrading the `DispatchR.Mediator` package (not shown as outdated by `dotnet list package --outdated`)

## Decisions

### Upgrade to `net10.0` target framework

`Microsoft.AspNetCore.*` and `Microsoft.EntityFrameworkCore.*` at version 10.x only support `net10.0`. Staying on `net9.0` would force us to skip those upgrades entirely, leaving the template on a soon-to-be-EOL runtime. Upgrading the target framework is the correct move.

**Alternative considered:** Stay on `net9.0` and only take minor/patch bumps. Rejected — too many packages are version-locked to their major .NET release, and `net9.0` support ends May 2025.

### Upgrade `FluentValidation` to 12.x

FluentValidation 12 removes the `FluentValidation.AspNetCore` auto-validation integration that was previously registered via `AddFluentValidation()`. Manual validation through MediatR pipeline behaviors (already the pattern in this template) is unaffected. The `AddFluentValidation` call in `Startup.cs` must be replaced with `AddValidatorsFromAssembly()` + validator-only DI registration.

**Alternative considered:** Pin at 11.x. Rejected — the template should ship current.

### Upgrade `Mapster` to 10.x

Mapster 10 is a major rewrite with API changes. The template's usage is minimal (TypeAdapterConfig registration in Startup and mapping calls in query handlers). Any renamed or removed members need to be fixed; no architectural changes are needed.

### Upgrade `Swashbuckle.AspNetCore` to 10.x

Swashbuckle 10 drops the legacy `AddSwaggerGen` + `UseSwagger` / `UseSwaggerUI` approach in favor of the built-in `Microsoft.AspNetCore.OpenApi` endpoint. However, since `Microsoft.AspNetCore.OpenApi` is already a dependency, the simplest fix is to update the existing Swashbuckle calls to the new 10.x API surface and keep Swashbuckle for its richer UI. Evaluate at implementation time whether removing Swashbuckle in favor of the built-in OpenAPI endpoint is cleaner.

### `coverlet.collector` 10.x and `Microsoft.NET.Test.Sdk` 18.x

Both are test infrastructure. No source changes expected; version bump only.

## Risks / Trade-offs

- **FluentValidation 12 DI registration change** → Review `Startup.cs`; replace removed helpers with supported alternatives. Pipeline behavior usage is unaffected.
- **Mapster 10 API surface change** → If specific TypeAdapterConfig or ProjectTo calls break, consult Mapster 10 migration guide. The template's usage is small so breakage is likely limited.
- **Swashbuckle 10 / OpenAPI endpoint changes** → The new `app.MapOpenApi()` / `app.UseSwaggerUI()` setup differs from 7.x. If Swashbuckle 10 is incompatible with the current setup, fall back to removing Swashbuckle and using `Microsoft.AspNetCore.OpenApi` only.
- **`net10.0` SDK requirement** → Developers must have .NET 10 SDK installed. Dockerfile base images must reference `mcr.microsoft.com/dotnet/aspnet:10.0` / `mcr.microsoft.com/dotnet/sdk:10.0`.

## Migration Plan

1. Update `<TargetFramework>` from `net9.0` to `net10.0` in all four `.csproj` files.
2. Update all `<PackageReference>` versions to their latest stable per `dotnet list package --outdated`.
3. Run `dotnet build` and fix any compilation errors introduced by breaking changes (FluentValidation, Mapster, Swashbuckle are the highest-risk).
4. Run `dotnet test` and fix any test failures.
5. Update Dockerfile base images to .NET 10 variants.

**Rollback:** revert the `.csproj` changes via git. No data migration is involved.

## Why

All NuGet packages in the template are pinned to versions that are now outdated, with several Microsoft packages lagging behind .NET 10 releases. Upgrading keeps the generated template current, incorporates security patches, and ensures projects bootstrapped from this template start on modern, supported library versions.

## What Changes

- **BREAKING** Upgrade all `Microsoft.AspNetCore.*` and `Microsoft.EntityFrameworkCore.*` packages from 9.x to 10.x (requires upgrading target framework from `net9.0` to `net10.0`)
- **BREAKING** Upgrade `FluentValidation` / `FluentValidation.AspNetCore` from 11.x to 12.x
- **BREAKING** Upgrade `Mapster` from 7.x to 10.x (both `Tusk.Api` and `Tusk.Application`)
- **BREAKING** Upgrade `Swashbuckle.AspNetCore` and `Swashbuckle.AspNetCore.Newtonsoft` from 7.x to 10.x
- **BREAKING** Upgrade `coverlet.collector` from 6.x to 10.x
- **BREAKING** Upgrade `Microsoft.NET.Test.Sdk` from 17.x to 18.x
- Upgrade `CSharpFunctionalExtensions` from 3.4.3 to 3.7.0
- Upgrade `Serilog` from 4.2.0 to 4.3.1
- Upgrade `Serilog.AspNetCore` from 9.0.0 to 10.0.0
- Upgrade `Serilog.Sinks.Console` from 6.0.0 to 6.1.1
- Upgrade `Newtonsoft.Json` from 13.0.3 to 13.0.4
- Upgrade `FluentAssertions` from 8.0.1 to 8.10.0
- Upgrade `xunit.runner.visualstudio` from 3.0.2 to 3.1.5
- Upgrade `Microsoft.CodeAnalysis.NetAnalyzers` from 9.0.0 to 10.0.301
- Update `<TargetFramework>` from `net9.0` to `net10.0` in all `.csproj` files

## Capabilities

### New Capabilities

- `package-versions`: Pinned package versions across all projects in the template reflect current latest releases

### Modified Capabilities

- `cqrs-dispatching`: FluentValidation major version upgrade may affect validator registration and behavior in the MediatR pipeline

## Impact

- All four `.csproj` files: `Tusk.Api`, `Tusk.Application`, `Tusk.Domain`, `Tusk.Api.Tests`
- Build pipeline: target framework change requires .NET 10 SDK on CI/CD and developer machines
- Dockerfile: base image will need to reference .NET 10 runtime/SDK images
- No public API surface changes — this is template infrastructure only

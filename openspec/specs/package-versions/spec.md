### Requirement: Template targets .NET 10
All projects in the template SHALL target `net10.0` as their framework moniker.

#### Scenario: Build succeeds on .NET 10 SDK
- **WHEN** `dotnet build` is run with the .NET 10 SDK installed
- **THEN** all projects compile without errors or warnings about target framework mismatch

### Requirement: All packages at latest stable version
Every `<PackageReference>` in the template SHALL be pinned to the latest stable version available at the time of this change, as determined by `dotnet list package --outdated`.

#### Scenario: No outdated packages reported
- **WHEN** `dotnet list package --outdated` is run against the solution
- **THEN** no packages are reported as having a newer stable version

### Requirement: Template builds and tests pass after upgrade
After all package versions are updated the solution SHALL build cleanly and the full test suite SHALL pass.

#### Scenario: dotnet build succeeds
- **WHEN** `dotnet build` is run on the solution
- **THEN** the build exits with code 0 and reports zero errors

#### Scenario: dotnet test passes
- **WHEN** `dotnet test` is run on the solution
- **THEN** all tests pass and the exit code is 0

### Requirement: Dockerfile references .NET 10 base images
The template Dockerfile SHALL reference `mcr.microsoft.com/dotnet/aspnet:10.0` as the runtime image and `mcr.microsoft.com/dotnet/sdk:10.0` as the build image.

#### Scenario: Docker image builds successfully
- **WHEN** `docker build` is run using the template Dockerfile
- **THEN** the image builds without errors using .NET 10 base images

## Why

AutoMapper introduces significant runtime overhead through reflection-based mapping and a heavy configuration bootstrapping step; Mapster is a faster, lighter alternative with a simpler API and first-class support for source-generated adapters. Replacing it reduces startup time and allocations for projects generated from this template.

## What Changes

- Remove `AutoMapper` and `AutoMapper.Extensions.Microsoft.DependencyInjection` NuGet packages from `Tusk.Application` and `Tusk.Api`
- Add `Mapster` and `Mapster.DependencyInjection` NuGet packages
- Replace `Profile` subclasses in `GetStoryQuery.cs` and `GetAllStoriesQuery.cs` with Mapster `IRegister` implementations
- Replace `IMapper` injection and `.ProjectTo<T>(mapper.ConfigurationProvider)` calls with Mapster's `.ProjectToType<T>()` queryable extension
- Replace `services.AddAutoMapper(...)` in `Startup.cs` with `services.AddMapster()` and scan-based register wiring
- Update test helper `FakeFactory.GetMapper` and `UserStoryTests.cs` to build a Mapster `IMapper` instance instead of an AutoMapper `Mapper`

## Capabilities

### New Capabilities

- `object-mapping`: Object-to-object mapping configuration (entity → DTO) using Mapster type adapters and queryable projection

### Modified Capabilities

<!-- No external API or behavior changes — this is a library swap with identical mapping semantics -->

## Impact

- `template/src/Tusk.Application/Tusk.Application.csproj` — package swap
- `template/src/Tusk.Api/Tusk.Api.csproj` — package swap
- `template/src/Tusk.Application/Stories/Queries/GetStoryQuery.cs` — mapping config + handler
- `template/src/Tusk.Application/Stories/Queries/GetAllStoriesQuery.cs` — mapping config + handler
- `template/src/Tusk.Api/Startup.cs` — DI registration
- `template/tests/Tusk.Api.Tests/Common/FakeFactory.cs` — test mapper factory
- `template/tests/Tusk.Api.Tests/UserStories/UserStoryTests.cs` — test setup

## 1. Swap NuGet Packages

- [x] 1.1 Remove `AutoMapper` and `AutoMapper.Extensions.Microsoft.DependencyInjection` from `template/src/Tusk.Application/Tusk.Application.csproj` and add `Mapster`
- [x] 1.2 Remove `AutoMapper` references from `template/src/Tusk.Api/Tusk.Api.csproj` and add `Mapster.DependencyInjection`
- [x] 1.3 Remove `AutoMapper` from `template/tests/Tusk.Api.Tests` project references and add `Mapster` and `MapsterMapper`

## 2. Update DI Registration

- [x] 2.1 Replace `services.AddAutoMapper(typeof(ITuskDbContext))` with `services.AddMapster()` and scan-register all `IRegister` implementations in `Startup.cs`

## 3. Migrate GetAllStoriesQuery

- [x] 3.1 Remove `using AutoMapper` and `using AutoMapper.QueryableExtensions` from `GetAllStoriesQuery.cs`
- [x] 3.2 Replace `UserStoriesProfile : Profile` with a `UserStoriesRegister : IRegister` class that configures the same mapping via `TypeAdapterConfig`
- [x] 3.3 Remove `IMapper mapper` from `GetAllStoriesQueryHandler` constructor and update `.ProjectTo<UserStoriesDto>(mapper.ConfigurationProvider)` to `.ProjectToType<UserStoriesDto>()`

## 4. Migrate GetStoryQuery

- [x] 4.1 Remove `using AutoMapper` and `using AutoMapper.QueryableExtensions` from `GetStoryQuery.cs`
- [x] 4.2 Replace `UserStoryProfile : Profile` with a `UserStoryRegister : IRegister` class that configures all custom member mappings via `TypeAdapterConfig`
- [x] 4.3 Remove `IMapper mapper` from `GetStoryQueryHandler` constructor and update `.ProjectTo<UserStoryDto>(mapper.ConfigurationProvider)` to `.ProjectToType<UserStoryDto>()`

## 5. Update Test Infrastructure

- [x] 5.1 Replace `FakeFactory.GetMapper(IEnumerable<Profile>)` with a helper that accepts `IEnumerable<IRegister>`, builds a scoped `TypeAdapterConfig`, applies each register, and returns a `MapsterMapper.Mapper` instance
- [x] 5.2 Update `UserStoryTests.cs` to pass `new UserStoriesRegister()` / `new UserStoryRegister()` instead of the old `Profile` subclasses to `FakeFactory.GetMapper`
- [x] 5.3 Remove remaining `using AutoMapper` statements from test files

## 6. Verify

- [x] 6.1 Run `dotnet build` from `template/` and confirm zero errors
- [x] 6.2 Run `dotnet test` from `template/` and confirm all tests pass

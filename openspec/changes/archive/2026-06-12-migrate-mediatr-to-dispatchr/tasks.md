## 1. Preparation

- [x] 1.1 Look up DispatchR NuGet package: confirm exact interface names (`IRequest<T>`, `INotification`, `IPipelineBehavior` equivalents), dispatcher interface, and DI registration method
- [x] 1.2 Confirm whether DispatchR supports open-generic pipeline behavior registration (used by `EventLoggerBehavior` and `RequestPerformanceBehavior`)

## 2. Package Swap

- [x] 2.1 In `template/src/Tusk.Application/Tusk.Application.csproj`: remove `MediatR` package reference, add `DispatchR` package reference with the current stable version
- [x] 2.2 Run `dotnet restore` from `template/` and confirm no MediatR package is pulled in transitively

## 3. Application Layer — Interfaces

- [x] 3.1 Update `template/src/Tusk.Application/Stories/Commands/CreateStoryCommand.cs`: replace `IRequest<int>`, `IRequestHandler<,>`, and `IMediator` usages with DispatchR equivalents; remove `using MediatR;`
- [x] 3.2 Update `template/src/Tusk.Application/Stories/Commands/ToggleDoneCommand.cs`: same interface swap
- [x] 3.3 Update `template/src/Tusk.Application/Stories/Queries/GetStoryQuery.cs`: replace `IRequest<UserStoryViewModel>`, `IRequestHandler<,>`; remove `using MediatR;`
- [x] 3.4 Update `template/src/Tusk.Application/Stories/Queries/GetAllStoriesQuery.cs`: same interface swap
- [x] 3.5 Update `template/src/Tusk.Application/Stories/Events/UserStoryAddedEvent.cs`: replace `INotification`, `INotificationHandler<T>`; remove `using MediatR;`

## 4. Application Layer — Behaviors

- [x] 4.1 Update `template/src/Tusk.Application/Behaviours/EventLoggerBehavior.cs`: replace `IPipelineBehavior<,>` and `RequestHandlerDelegate<TResponse>` with DispatchR equivalents; remove `using MediatR;`
- [x] 4.2 Update `template/src/Tusk.Application/Behaviours/RequestPerformanceBehavior.cs`: same behavior interface swap

## 5. API Layer

- [x] 5.1 Update `template/src/Tusk.Api/Startup.cs`: replace `services.AddMediatR(...)` with DispatchR DI registration; replace `IPipelineBehavior<,>` open-generic registrations if needed; remove `using MediatR;`
- [x] 5.2 Update `template/src/Tusk.Api/Controllers/BaseController.cs`: replace `IMediator`/`ISender` injection with the DispatchR dispatcher interface; remove `using MediatR;`

## 6. Tests

- [x] 6.1 Update `template/tests/Tusk.Api.Tests/Common/FakeFactory.cs`: replace any MediatR mocks or registrations with DispatchR equivalents; remove `using MediatR;`

## 7. Verification

- [x] 7.1 Run `dotnet build` from `template/` — build must succeed with zero errors
- [x] 7.2 Run `dotnet test` from `template/` — all tests must pass
- [x] 7.3 Run `grep -r "MediatR" template/ --include="*.cs" --include="*.csproj"` — output must be empty (zero remaining references)
- [x] 7.4 Build and run the Docker image (`docker build` + `docker run`) and hit at least one endpoint to confirm runtime dispatch works

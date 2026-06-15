## Why

MediatR 12.x is a reflection-heavy library with a large footprint; DispatchR is a modern, lightweight alternative designed for .NET with a simpler API surface. Migrating reduces indirect dependencies, aligns the template with current .NET ecosystem trends, and can improve startup performance and AOT compatibility.

## What Changes

- Replace the `MediatR` NuGet package with `DispatchR` in `Tusk.Application.csproj`
- Replace all MediatR interfaces (`IRequest<T>`, `IRequestHandler<,>`, `INotification`, `INotificationHandler<T>`, `IPipelineBehavior<,>`, `IMediator`/`ISender`) with their DispatchR equivalents
- Replace `services.AddMediatR(...)` registration in `Startup.cs` with the DispatchR registration call
- Re-implement pipeline behaviors (`EventLoggerBehavior`, `RequestPerformanceBehavior`) using DispatchR's behavior API
- Update `BaseController` and any service that injects `IMediator`/`ISender` to use the DispatchR dispatcher interface
- Update event publishing in `CreateStoryCommandHandler` (`mediator.Publish(...)`) to use the DispatchR equivalent
- Update test infrastructure in `FakeFactory.cs` accordingly
- Remove `using MediatR;` across all affected files

## Capabilities

### New Capabilities

- `cqrs-dispatching`: Request dispatching and notification publishing via DispatchR instead of MediatR — same CQRS semantics, different implementation contract

### Modified Capabilities

*(none — no spec-level behavioral requirements change; only the implementation library changes)*

## Impact

- **Tusk.Application.csproj**: package reference swap (`MediatR` → `DispatchR`)
- **Tusk.Application** — all command, query, event, and behavior files: interface and using-statement changes
- **Tusk.Api** — `Startup.cs`, `BaseController.cs`: DI registration and dispatcher injection changes
- **Tusk.Api.Tests** — `FakeFactory.cs`: test wiring update
- **Breaking for template consumers**: any project generated from this template before this change will need to migrate manually

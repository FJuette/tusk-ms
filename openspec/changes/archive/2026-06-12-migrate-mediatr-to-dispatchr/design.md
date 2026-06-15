## Context

The template currently depends on `MediatR 12.4.1` for CQRS dispatching. All commands, queries, events, and pipeline behaviors implement MediatR interfaces (`IRequest<T>`, `IRequestHandler<,>`, `INotification`, `INotificationHandler<T>`, `IPipelineBehavior<,>`). The dispatcher is injected as `IMediator` in controllers and handlers.

DispatchR is a lightweight NuGet alternative to MediatR. The migration is a straight interface-swap across `Tusk.Application`, `Tusk.Api`, and the test project — no behavioral changes.

## Goals / Non-Goals

**Goals:**
- Remove all MediatR references; the project must compile with zero MediatR packages
- Maintain the same CQRS semantics: command dispatch, query dispatch, notification publish
- Keep the two pipeline behaviors (`EventLoggerBehavior`, `RequestPerformanceBehavior`) functional
- Keep the template parameter `--DisableAuthentication` unaffected

**Non-Goals:**
- Changing how FluentValidation, AutoMapper, or EF are used
- Altering business logic in any command or query handler
- Adding new behaviors or capabilities beyond what exists today

## Decisions

### D1 — One-to-one interface mapping

MediatR interfaces map to DispatchR equivalents; all other code stays identical.

| MediatR | DispatchR |
|---|---|
| `IRequest<TResponse>` | DispatchR equivalent (verify from NuGet docs) |
| `IRequestHandler<TRequest, TResponse>` | DispatchR equivalent |
| `INotification` | DispatchR equivalent |
| `INotificationHandler<T>` | DispatchR equivalent |
| `IPipelineBehavior<TRequest, TResponse>` | DispatchR equivalent |
| `RequestHandlerDelegate<TResponse>` | DispatchR equivalent |
| `IMediator` / `ISender` | DispatchR dispatcher interface |
| `services.AddMediatR(...)` | DispatchR DI registration call |

> **Action required before implementation**: Confirm exact interface names from the [DispatchR NuGet page](https://www.nuget.org/packages/DispatchR) or its GitHub README.

**Rationale**: A mechanical swap avoids any risk of accidentally altering behavior; all tests continue to cover the same logic.

### D2 — Update package reference in Tusk.Application.csproj only

MediatR is only a direct dependency of `Tusk.Application`. No other `.csproj` references it directly.

**Rationale**: Keeps the change minimal; `Tusk.Api` and tests reference Application indirectly.

### D3 — Keep behaviors as typed open-generics

Both behaviors are registered as `typeof(IPipelineBehavior<,>)` open generics. Use the DispatchR equivalent registration pattern.

**Rationale**: Avoids enumerating every request type explicitly.

## Risks / Trade-offs

- **Unknown DispatchR API surface** → Verify exact interface and registration names before writing any code; the design assumes a conventional MediatR-like API.
- **No official migration guide** → Low risk for a simple template with few files; a manual find-and-replace is feasible.
- **Pipeline behavior API may differ** → If DispatchR does not support open-generic pipeline behaviors, the two behaviors must be registered differently or re-implemented. Flag this during implementation.
- **Template consumers must migrate manually** → Document the breaking change in the template changelog/README.

## Migration Plan

1. Update `Tusk.Application.csproj`: remove `MediatR`, add `DispatchR`.
2. Update all `using MediatR;` statements across Application, Api, and Tests.
3. Replace interfaces file by file (commands → queries → events → behaviors → controllers → test wiring).
4. Update DI registration in `Startup.cs`.
5. Run `dotnet build` — resolve any compilation errors.
6. Run `dotnet test` — all tests must pass.
7. Smoke-test via Docker (`docker run --rm ...`) to confirm runtime behaviour.

## Open Questions

- What are the exact DispatchR interface names? (confirm from NuGet/GitHub before implementation)
- Does DispatchR support open-generic pipeline behaviors? If not, what is the alternative?
- Does DispatchR have a direct equivalent to `IMediator.Publish` for notifications?

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

A `dotnet new` project template for C# REST APIs in DDD/Onion-Architecture style. The root contains packaging config; all actual template source lives under `template/`.

## Commands

All commands run from `template/` unless noted.

```bash
# Run (hot-reload)
dotnet watch run --project ./src/Tusk.Api/Tusk.Api.csproj

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~InMemoryUserStoryTests"

# EF migration (from template/)
cd ./src/Tusk.Api && dotnet ef migrations add <name> --context TuskDbContext
cd ./src/Tusk.Api && dotnet ef database update --context TuskDbContext

# Docker (runs on :5400)
docker build -t tusk-ms .
docker run --rm -p 5400:8080 -e CONNECTION_STRING=dummy -e ASPNETCORE_ENVIRONMENT=Development tusk-ms

# Pack the nuget template (from repo root)
dotnet pack nuget.csproj

# Install template locally for testing
dotnet new install ./            # from template/
dotnet new ddd-webapi -n MyApp   # create a project from it
dotnet new ddd-webapi -n MyApp --DisableAuthentication true
```

## Architecture

Three-layer Onion / CQRS:

```
Tusk.Domain          ← entities, value objects, enumeration pattern classes
Tusk.Application     ← MediatR commands, queries, validators, DTOs, AutoMapper profiles
Tusk.Api             ← controllers, EF DbContext, startup wiring, infrastructure
tests/Tusk.Api.Tests ← XUnit + FluentAssertions integration tests (InMemory EF)
```

Dependency direction: `Api → Application → Domain`. `Application` only knows `ITuskDbContext`; the concrete `TuskDbContext` lives in `Api`.

## Key patterns

**CQRS via MediatR** — every feature is a `record` implementing `IRequest<T>`, its handler, and its FluentValidation validator, all in one file (e.g. `CreateStoryCommand.cs`). AutoMapper profiles for query DTOs live in the same file as the query.

**Entity ownership / row-level security** — `EntityBase` implements `IOwnedBy`. `TuskDbContext` has a global EF query filter `x.OwnedBy == _userId` on `UserStory`, and `SaveChanges` auto-stamps `OwnedBy` on newly added entities. The `_userId` comes from `IGetClaimsProvider` injected at construction time.

**Value objects** — use `CSharpFunctionalExtensions.ValueObject` (see `Priority`). Factory methods return `Result<T>`; use `.Value` only after confirming success or in tests.

**Enumeration pattern** — `BusinessValue` is an `EntityBase` subclass with static readonly instances instead of a plain enum. EF seed data must match the static IDs. Mark enumeration-type entries as `EntityState.Unchanged` in `SaveChanges` (already handled in `TuskDbContext`).

**Environment / config** — `EnvFactory` reads required env vars and throws `MissingEnvException` if absent; pass a fallback to make a var optional. `CONNECTION_STRING` is required only in Production. Non-production environments use an InMemory EF database.

**Exception → HTTP mapping** — `CustomExceptionFilter` maps `NotFoundException → 404`, `ValidationException / InvalidOperationException → 400`, everything else → 500. Validation errors are returned as a `{ propertyName: [messages] }` dictionary. Stack traces are included only outside Production.

**MediatR pipeline behaviors** — `EventLoggerBehavior` and `RequestPerformanceBehavior` are registered for all requests; add new cross-cutting concerns there.

**Authentication** — JWT Bearer auth is wired in `Startup.cs` inside `#if (!DisableAuthentication)` blocks, controlled by the `--DisableAuthentication` template parameter. The default policy requires a `modules` claim.

## Template parameter

`--DisableAuthentication` (bool, default `false`) — strips JWT auth from generated output via conditional compilation blocks in `Startup.cs`.

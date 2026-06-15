## 1. Database Isolation Bug Fix

- [x] 1.1 In `TuskDbContext.OnConfiguring`, replace `new Guid().ToString()` with `Guid.NewGuid().ToString()` so each non-production context gets a unique in-memory database

## 2. CreateStory Command Fix

- [x] 2.1 In `CreateStoryCommandHandler.Handle`, look up the `BusinessValue` entity from context using `request.BusinessValue` as the id (use `context.BusinessValues.FindAsync`)
- [x] 2.2 Pass the resolved `BusinessValue` entity and `request.Importance` to the `UserStory` constructor instead of the hardcoded `BusinessValue.BV1000` and default relevance
- [x] 2.3 Update the `Create_Success_ReturnsStoryId` test in `UserStoryTests` to assert that the persisted story has the `BusinessValue` and `Importance` that were sent in the command

## 3. ToggleDone Command Fix

- [x] 3.1 In `ToggleDoneCommandHandler.Handle`, delete the `context.Attach(story)` call — the entity is already tracked by the context after `SingleOrDefaultAsync`

## 4. Exception Filter Fix

- [x] 4.1 In `CustomExceptionFilter.OnException`, extract the `.Value` property from `returnMessage` before embedding it in the non-production response object so the error payload serializes as plain JSON instead of a `JsonResult` wrapper

## 5. Pipeline Behavior Naming

- [x] 5.1 In `EventLoggerBehavior`, replace the `"EventLoggerBehavior"` string literal with `nameof(EventLoggerBehavior)` in all `Log.*` calls
- [x] 5.2 In `RequestPerformanceBehavior`, replace the `"RequestPerformanceBehavior"` string literal with `nameof(RequestPerformanceBehavior)` in all `Log.*` calls

## 6. CORS Configuration Fix

- [x] 6.1 In `Startup.ConfigureServices`, wrap `builder.AllowAnyOrigin()` in an `if (env.IsDevelopment())` branch and move `builder.WithOrigins(...)` to the else branch so the two directives never coexist

## 7. Identity Fallback Fix

- [x] 7.1 In `GetClaimsFromUser`, change the anonymous-user fallback from `"Admin"` to `string.Empty` (or a neutral constant like `"anonymous"`) with a comment explaining why

## 8. FluentValidation Version Pin

- [x] 8.1 In `Tusk.Application.csproj`, downgrade `FluentValidation` from `12.0.0-preview1` to the latest stable `11.x` release compatible with `FluentValidation.AspNetCore 11.3.0` in `Tusk.Api.csproj`
- [x] 8.2 Run `dotnet build` from `template/` to confirm the solution compiles cleanly after the version change

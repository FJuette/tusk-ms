using Microsoft.Extensions.Diagnostics.HealthChecks;
using Tusk.Api.Persistence;
using Tusk.Application.Persistence;

namespace Tusk.Api.Health;
public class ApiHealthCheck : IHealthCheck
{
    private readonly ITuskDbContext _ctx;

    public ApiHealthCheck(
        ITuskDbContext ctx) =>
        _ctx = ctx;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var canConnect = await _ctx.Database.CanConnectAsync(cancellationToken);
        if (canConnect)
        {
            return HealthCheckResult.Healthy("Database connection is working.");
        }

        return HealthCheckResult.Unhealthy("Database connection is not working.");
    }
}

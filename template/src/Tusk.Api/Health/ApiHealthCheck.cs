using Microsoft.Extensions.Diagnostics.HealthChecks;
using Tusk.Application.Persistence;

namespace Tusk.Api.Health;

public class ApiHealthCheck(ITuskDbContext ctx) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var canConnect = await ctx.Database.CanConnectAsync(cancellationToken);
        return canConnect
            ? HealthCheckResult.Healthy("Database connection is working.")
            : HealthCheckResult.Unhealthy("Database connection is not working.");
    }
}

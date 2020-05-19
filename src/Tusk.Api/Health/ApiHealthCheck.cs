using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Tusk.Api.Persistence;

namespace Tusk.Api.Health
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly TuskDbContext _ctx;
        public ApiHealthCheck(TuskDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await _ctx.Database.CanConnectAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database connection is working.");
        }
    }
}

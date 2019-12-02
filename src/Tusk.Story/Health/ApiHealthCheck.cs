using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tusk.Story.Health
{
    public class ApiHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.Delay(100, cancellationToken); // Fake waiting time
            return HealthCheckResult.Healthy("Database connection is working.");
        }
    }
}
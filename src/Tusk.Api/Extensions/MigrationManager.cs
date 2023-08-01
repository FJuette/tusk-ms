using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Tusk.Api.Persistence;
using Tusk.Application.Persistence;

namespace Tusk.Api.Extensions;
public static class MigrationManager
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
    public static IHost MigrateDatabase(
        this IHost webHost)
    {
        using var scope = webHost.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<ITuskDbContext>();
        try
        {
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            if (env.IsProduction() || env.IsStaging())
            {
                // not working with in memory dbs only relational dbs
                appContext.Database.Migrate();
            }
            else
            {
                appContext.Database.EnsureCreated();
            }

            new SampleDataSeeder(appContext).SeedAll();
        }
        catch (Exception)
        {
            Environment.Exit(-110);
        }

        return webHost;
    }
}

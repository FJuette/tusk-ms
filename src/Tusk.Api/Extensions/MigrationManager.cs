using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Tusk.Api.Persistence;

namespace Tusk.Api.Extensions;
public static class MigrationManager
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
    public static IHost MigrateDatabase(
        this IHost webHost)
    {
        using var scope = webHost.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<TuskDbContext>();
        try
        {
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            appContext.Database.EnsureCreated();
            if (env.IsProduction())
            {
                // not working with in memory dbs
                appContext.Database.Migrate();
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

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tusk.Api.Persistence;

namespace Tusk.Api.Extensions
{
    public static class MigrationManager
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Log errors or do anything you think it's needed
                throw;
            }

            return webHost;
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tusk.Story.Persistence;

namespace Tusk.Story.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<TuskDbContext>())
                {
                    try
                    {
                        appContext.Database.EnsureCreated();
                        // appContext.Database.Migrate(); // not working with in memory dbs atm
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }

            return webHost;
        }
    }
}

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tusk.Story.Persistance;

namespace Tusk.Story.Tests.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                
                services.AddDbContext<TuskDbContext>(options =>
                {
                    options.UseInMemoryDatabase(new Guid().ToString());
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();


                // see https://docs.microsoft.com/de-de/aspnet/core/test/integration-tests?view=aspnetcore-3.0 for more
                // Create a scope to obtain a reference to the database
                // context (NorthwindDbContext)
                //using (var scope = sp.CreateScope())
                //{
                //    var scopedServices = scope.ServiceProvider;
                //    var context = scopedServices.GetRequiredService<ITuskDbContext>();
                //    var logger = scopedServices
                //        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                //    var concreteContext = (TuskDbContext)context;

                //    // Ensure the database is created.
                //    concreteContext.Database.EnsureCreated();

                //    try
                //    {
                //        // Seed the database with test data.
                //        Utilities.InitializeDbForTests(concreteContext);
                //    }
                //    catch (Exception ex)
                //    {
                //        logger.LogError(ex, $"An error occurred seeding the " +
                //                            "database with test messages. Error: {ex.Message}");
                //    }
                //}
            });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Tusk.Api.Infrastructure;
using Tusk.Api.Persistence;

namespace Tusk.Api.Tests.Common
{
    public class TuskContextFactory
    {
        public static TuskDbContext Create()
        {
            var options = new DbContextOptionsBuilder<TuskDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TuskDbContext(null, null);

            context.Database.EnsureCreated();

            context.Customers.AddRange(new[] {
                new Customer { CustomerId = "ADAM", ContactName = "Adam Cogan" },
                new Customer { CustomerId = "JASON", ContactName = "Jason Taylor" },
                new Customer { CustomerId = "BREND", ContactName = "Brendan Richards" },
            });

            context.Orders.Add(new Order
            {
                CustomerId = "BREND"
            });

            context.SaveChanges();

            return context;
        }

        public static void Destroy(TuskDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}

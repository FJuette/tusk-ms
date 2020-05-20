using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tusk.Api.Infrastructure;
using Tusk.Api.Models;

namespace Tusk.Api.Persistence
{
#nullable disable
    public class TuskDbContext : DbContext
    {
        private static readonly Type[] EnumerationTypes = {  }; // e.g. typeof(Cluster)
        private readonly string _userId;
        private readonly IWebHostEnvironment _env;

        public TuskDbContext(IWebHostEnvironment env, IGetClaimsProvider userData)
        {
            _env = env;
            _userId = userData.UserId;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_env.EnvironmentName == "Production")
                optionsBuilder.UseSqlServer(EnvFactory.GetConnectionString());
            else
            {
                optionsBuilder.UseInMemoryDatabase(new Guid().ToString());
                optionsBuilder.EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }

        // No DbSet for UserTask here. They must set within a user story
        public DbSet<UserStory> Stories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserStory>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                b.Property(e => e.AcceptanceCriteria)
                    .HasMaxLength(int.MaxValue);
                b.Property(c => c.Importance)
                    .HasConversion<int>(); // 'string' ist possible too, for more see https://medium.com/agilix/entity-framework-core-enums-ee0f8f4063f2
                b.HasMany(e => e.StoryTasks)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                // Mapping for ValueObject
                b.Property(e => e.Priority).HasConversion(p => p.Value, p => Priority.Create(p).Value);

                b.HasQueryFilter(x => x.OwnedBy == _userId);

                b.HasData(new {
                    Id = 1,
                    Title = "My first story",
                    Priority = Priority.Create(1).Value,
                    Text = "This is my content",
                    AcceptanceCriteria = "This must be fulfilled",
                    Importance = UserStory.Relevance.ShouldHave,
                    OwnedBy = "Admin"
                });
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            MarkEnumTypesAsUnchanged();
            // TODO FILL ID FROM user
            this.MarkCreatedItemAsOwnedBy("Admin");
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            MarkEnumTypesAsUnchanged();
            // TODO FILL ID FROM user
            this.MarkCreatedItemAsOwnedBy("Admin");
            return base.SaveChanges();
        }

        private void MarkEnumTypesAsUnchanged()
        {
            IEnumerable<EntityEntry> enumerationEntries =
                ChangeTracker.Entries().Where(x => TuskDbContext.EnumerationTypes.Contains(x.Entity.GetType()));

            foreach (EntityEntry enumerationEntry in enumerationEntries)
            {
                enumerationEntry.State = EntityState.Unchanged;
            }
        }
    }

    public static class ContextExtensions
    {
        public static void MarkCreatedItemAsOwnedBy(this DbContext context, string userId)
        {
            foreach (var entityEntry in context.ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Added))
            {
                if (entityEntry.Entity is IOwnedBy entityToMark)
                {
                    entityToMark.SetOwnedBy(userId);
                }
            }
        }
    }
}

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
using Microsoft.Extensions.Hosting;

namespace Tusk.Api.Persistence
{
    #nullable disable
    public class TuskDbContext : DbContext
    {
        private static readonly Type[] _enumerationTypes = { typeof(BusinessValue) };
        private readonly string _userId;
        private readonly IWebHostEnvironment _env;

        public TuskDbContext(IWebHostEnvironment env, IGetClaimsProvider userData)
        {
            _env = env;
            _userId = userData?.UserId;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_env.IsProduction())
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
        public DbSet<BusinessValue> BusinessValues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder?.Entity<UserStory>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                b.Property(e => e.Text)
                    .HasMaxLength(int.MaxValue);
                b.Property(e => e.AcceptanceCriteria)
                    .HasMaxLength(int.MaxValue);

                // 'string' ist possible too, for more see https://medium.com/agilix/entity-framework-core-enums-ee0f8f4063f2
                b.Property(c => c.Importance)
                    .HasConversion<int>();
                b.HasMany(e => e.StoryTasks)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasOne(e => e.BusinessValue).WithMany();

                // Mapping for ValueObject
                b.Property(e => e.Priority)
                    .HasConversion(p => p.Value, p => Priority.Create(p).Value);

                b.HasQueryFilter(x => x.OwnedBy == _userId);

            });

            builder?.Entity<StoryTask>(b =>
            {
                b.Property(e => e.Description)
                    .HasMaxLength(int.MaxValue);
            });

            builder?.Entity<BusinessValue>(b =>
            {
                b.Property(p => p.Name);
                b.HasData(new
                {
                    Id = 1,
                    Name = "Business Value 1000",
                    OwnedBy = "Admin"
                });
                b.HasData(new
                {
                    Id = 2,
                    Name = "Business Value 900",
                    OwnedBy = "Admin"
                });
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            MarkEnumTypesAsUnchanged();
            this.MarkCreatedItemAsOwnedBy(_userId);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            MarkEnumTypesAsUnchanged();
            this.MarkCreatedItemAsOwnedBy(_userId);
            return base.SaveChanges();
        }

        private void MarkEnumTypesAsUnchanged()
        {
            IEnumerable<EntityEntry> enumerationEntries =
                ChangeTracker.Entries().Where(x => _enumerationTypes.Contains(x.Entity.GetType()));

            foreach (EntityEntry enumerationEntry in enumerationEntries)
            {
                enumerationEntry.State = EntityState.Unchanged;
            }
        }
    }

    public static class ContextExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
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

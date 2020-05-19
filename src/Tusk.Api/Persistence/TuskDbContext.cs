using System;
using GenFu;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

                // Generate some demo data
                GenFu.GenFu.Configure<UserStory>().Fill(p => p.Id).WithinRange(1, 100000);
                GenFu.GenFu.Configure<UserStory>().Fill(p => p.AcceptanceCriteria).AsLoremIpsumSentences(2);
                var story = A.ListOf<UserStory>(5);
                b.HasData(story);
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Tusk.Story.Models;

namespace Tusk.Story.Persistence
{
    public class TuskDbContext : DbContext
    {
        public TuskDbContext(DbContextOptions options) : base(options)
        {
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
            });
        }
    }
}

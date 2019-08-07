using Microsoft.EntityFrameworkCore;
using Tusk.Story.Models;

namespace Tusk.Story.Persistance
{
    public class TuskDbContext : DbContext
    {
        public TuskDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<UserStory> Stories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<UserStory>()
                .HasKey(e => e.Id);

            builder.Entity<UserStory>()
                .Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<UserStory>()
                .Property(e => e.AcceptanceCriteria)
                .HasMaxLength(int.MaxValue);
        }
    }
}
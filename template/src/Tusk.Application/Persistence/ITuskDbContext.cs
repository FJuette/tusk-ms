using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tusk.Domain;

namespace Tusk.Application.Persistence;

public interface ITuskDbContext : IDisposable
{
    // No DbSet for UserTask here. They must set within a user story
    public DbSet<UserStory> Stories { get; set; }
    public DbSet<BusinessValue> BusinessValues { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
        where TEntity : class;

    public DatabaseFacade Database { get; }
}

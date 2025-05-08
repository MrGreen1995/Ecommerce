using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangeOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                {
                    entry.Entity.LastModifiedBy = "John"; //TODO: Replace with auth server
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    break;
                }
                case EntityState.Added:
                {
                    entry.Entity.CreatedBy = "John"; //TODO: Replace with auth server
                    entry.Entity.CreatedDate = DateTime.Now;
                    break;
                }
            }
        }
        
        return base.SaveChangesAsync(acceptAllChangeOnSuccess, cancellationToken);
    }
}
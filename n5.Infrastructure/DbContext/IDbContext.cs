using Microsoft.EntityFrameworkCore;
using n5.Infrastructure.Models;

namespace n5.Infrastructure.DbContext;
public interface IMyApiDbContext
{
   DbSet<TEntity> Set<TEntity>() where TEntity : class;
   
   public void Rollback();

    public DbSet<Employees> Employees { get; set; }
    
    public DbSet<Permissions> Permissions { get; set; }

    public DbSet<PermissionTypes> PermissionTypes { get; set; }
    
   public void SaveChanges(CancellationToken cancellationToken = default);
   
}
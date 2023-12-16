
using n5.Infrastructure.Models;

namespace n5.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

public class N5DbContext : DbContext, IMyApiDbContext
{
    public new DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return base.Set<TEntity>();
    }

    public DbSet<Employees> Employees { get; set; }
    
    public DbSet<Permissions> Permissions { get; set; }

    public DbSet<PermissionTypes> PermissionTypes { get; set; }

    public N5DbContext(DbContextOptions<N5DbContext> options) : base(options)
    {
        // options contiene la cadena de conexión
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuraciones del modelo, relaciones, etc.
    }

    public void SaveChanges(CancellationToken cancellationToken = default)
    {
        SaveChanges();
    }

    public void Rollback()
    {
        Rollback();
    }
}

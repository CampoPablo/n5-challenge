using n5.Infrastructure.DbContext;
using n5.Infrastructure.Repository;

namespace n5.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly IMyApiDbContext _context;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(IMyApiDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    /// <summary>
    /// Implementation for committing the changes to the repository
    /// </summary>
    public void Commit()
    {
        _context.SaveChanges();
    }
  
    /// <summary>
    /// Implementation for rolling back the changes to the repository
    /// </summary>
    public void Rollback()
    {
        _context.Rollback();
    }

    /// <summary>
    /// Implementation for getting a repository by its type
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }
     
     
    public void Dispose()
    {

    }

    
}

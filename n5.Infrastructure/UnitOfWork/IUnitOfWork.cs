using n5.Infrastructure.Repository;

namespace n5.Infrastructure.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    void Commit();
    void Rollback();
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}
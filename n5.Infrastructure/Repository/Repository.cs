using System.Linq.Expressions;
using n5.Infrastructure.DbContext;

namespace n5.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        #region fields
        protected readonly IMyApiDbContext _context;
        //protected DbSet<T> _dbSet;
        #endregion

        public Repository(IMyApiDbContext context)
        {
            _context = context;
            //_dbSet = context.Set<T>();
        }

        /// <summary>
        /// Implementation for adding an entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            // Implementation for adding an entity to the repository
            _context.Set<T>().Add(entity);
        }

        /// <summary>
        /// Implementation for retrieving an entity by its ID from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Implementation for retrieving all entities from the repository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            // Implementation for retrieving all entities from the repository
            return _context.Set<T>().ToList();
        }

        /// <summary>
        /// Implementation for retrieving all entities from the repository
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        /// <summary>
        /// Implementation for adding a range of entities to the repository
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        /// <summary>
        /// Implementation for removing a range of entities from the repository
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Implementation for removing a range of entities from the repository
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
  
        /// <summary>
        /// Implementation for updating an entity from the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <summary>
        /// Implementation for updating an entity in the repository
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }  
    }
}

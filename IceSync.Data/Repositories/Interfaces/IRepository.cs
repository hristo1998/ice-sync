using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace IceSync.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetById(object id);

        Task<TEntity> GetByIdAsync(object id);

        void Add(TEntity entity);

        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);

        void Delete(object id);

        Task DeleteAsync(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DR.Services.Emails.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);
        TEntity Get(Guid id);
        TEntity Get(string id);
        TEntity Get(int id);
        Task<TEntity> GetAsync(Guid id);
        Task<TEntity> GetAsync(string id);
        Task<TEntity> GetAsync(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllQueryable();
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}

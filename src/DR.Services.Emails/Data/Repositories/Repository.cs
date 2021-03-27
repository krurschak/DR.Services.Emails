using DR.Services.Emails.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DR.Services.Emails.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly EFUnitOfWork unitOfWork;
        private readonly DbSet<TEntity> dbSet;

        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork as EFUnitOfWork;
            if (this.unitOfWork is null)
            {
                throw new ArgumentNullException("Unit of work must not be null for the repository.");
            }
            dbSet = this.unitOfWork.Context.Set<TEntity>();
        }

        public void Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("The entity must not be null.");
            }
            dbSet.Remove(entity);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("The entity must not be null.");
            }
            await Task.Run(() => dbSet.Remove(entity));
        }

        public TEntity Get(Guid id)
        {
            return dbSet.Find(id);
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public TEntity Get(string id)
        {
            return dbSet.Find(id);
        }

        public async Task<TEntity> GetAsync(string id)
        {
            return await dbSet.FindAsync(id);
        }

        public TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Single(predicate);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.SingleAsync(predicate);
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            return dbSet;
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.CountAsync(predicate);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException("The predicate must not be null.");
            }
            return dbSet.Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException("The predicate must not be null.");
            }
            return await dbSet.Where(predicate).AsQueryable().ToListAsync();
        }

        public void Insert(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException("Cannot insert an uninitialized entity.");
            }
            dbSet.Add(entity);
        }

        public async Task InsertAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException("Cannot insert an uninitialized entity.");
            }
            await dbSet.AddAsync(entity);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await unitOfWork.SaveChangesAsync();
        }
    }
}

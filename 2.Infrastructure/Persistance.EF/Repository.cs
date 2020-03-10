using Core;
using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistance.EF
{
    public class Repository<TEntity, TContext> : IRepository<TEntity, TContext> where TEntity : class, IEntity where TContext : DbContext
    {
        protected TContext _dbContext { get; private set; }

        protected IList<Expression<Func<TEntity, object>>> EagerIncludes { get; set; }

        public Repository(TContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
            if (EagerIncludes == null)
            {
                EagerIncludes = new List<Expression<Func<TEntity, object>>>();
            }
        }

        public virtual IQueryable<TEntity> Query()
        {
            return _dbContext.Set<TEntity>();
        }

        public virtual TEntity Get<TKey>(TKey id) where TKey : IComparable, IFormattable
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public virtual Task<TEntity> GetAsync<TKey>(TKey id) where TKey : IComparable, IFormattable
        {
            return _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual TEntity Create(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);

            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbContext.Set<TEntity>().Attach(entity);

                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual int Count()
        {
            return _dbContext.Set<TEntity>().Count();
        }

        public virtual IQueryable<TEntity> QueryEager()
        {
            throw new NotImplementedException("This method should be implemented in a specialized repository.");
        }
    }
}

// <copyright file="Repository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Persistance.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;
    using Core.Persistance;
    using Microsoft.EntityFrameworkCore;

    public class Repository<TEntity, TContext> : IRepository<TEntity, TContext> where TEntity : class, IEntity where TContext : DbContext
    {
        protected TContext DbContext { get; private set; }

        protected IList<Expression<Func<TEntity, object>>> EagerIncludes { get; set; }

        public Repository(TContext dbContext, IUnitOfWork unitOfWork)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.DbContext = dbContext;
            if (this.EagerIncludes == null)
            {
                this.EagerIncludes = new List<Expression<Func<TEntity, object>>>();
            }
        }

        public virtual IQueryable<TEntity> Query()
        {
            return this.DbContext.Set<TEntity>();
        }

        public virtual TEntity Get<TKey>(TKey id) where TKey : IComparable, IFormattable
        {
            return this.DbContext.Set<TEntity>().Find(id);
        }

        public virtual TEntity Create(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Add(entity);

            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            if (this.DbContext.Entry(entity).State == EntityState.Detached)
            {
                this.DbContext.Set<TEntity>().Attach(entity);

                this.DbContext.Entry(entity).State = EntityState.Modified;
            }

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Remove(entity);
        }

        public virtual int Count()
        {
            return this.DbContext.Set<TEntity>().Count();
        }

        public virtual IQueryable<TEntity> QueryEager()
        {
            throw new NotImplementedException("This method should be implemented in a specialized repository.");
        }

        public virtual bool Exist(int id)
        {
            throw new NotImplementedException("This method should be implemented in a specialized repository.");
        }
    }
}

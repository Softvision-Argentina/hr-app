// <copyright file="INonQuereableRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Persistance
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// This repository is suitable for wrapping connectors of non-relational databases, which may not offer the IQueryable collection.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity.</typeparam>
    public interface INonQuereableRepository<TEntity> : IRepository where TEntity : IEntity
    {
        // TODO: Change name of this interface to "INonQueryableRepository".
        IQueryable<TEntity> Query();

        IQueryable<TEntity> Query(int page, int count);

        IQueryable<TEntity> Query(Ordering<TEntity> ordering, int page, int count);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> searchExpression);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> searchExpression, int page, int count);

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> searchExpression,
            Ordering<TEntity> ordering,
            int page,
            int count);

        TEntity Get<TKey>(TKey id) where TKey : IComparable, IFormattable;

        Task<TEntity> GetAsync<TKey>(TKey id) where TKey : IComparable, IFormattable;

        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);

        int Count();
    }
}

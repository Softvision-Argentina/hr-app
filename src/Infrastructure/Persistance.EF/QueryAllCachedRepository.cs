// <copyright file="QueryAllCachedRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Persistance.EF
{
    using System;
    using System.Linq;
    using Core;
    using Core.Persistance;
    using Microsoft.EntityFrameworkCore;

    public class QueryAllCachedRepository<TEntity, TContext> : Repository<TEntity, TContext>
        where TEntity : class, IEntity where TContext : DbContext
    {
        private readonly IMemCache cache;

        public QueryAllCachedRepository(
            TContext dbContext,
            IUnitOfWork unitOfWork,
            IMemCache cache) : base(dbContext, unitOfWork)
        {
            this.cache = cache;
        }

        public override IQueryable<TEntity> Query()
        {
            var entityName = typeof(TEntity).Name;
            var queryableCacheKey = $"{entityName}_QueryAll";
            var enumCacheValue = (CacheGroup)Enum.Parse(typeof(CacheGroup), entityName);

            this.cache.TryGetValue(enumCacheValue, queryableCacheKey, out IQueryable<TEntity> queryAll);

            if (queryAll == null)
            {
                queryAll = base.Query().ToList().AsQueryable();
                this.cache.Set(enumCacheValue, queryableCacheKey, queryAll);
            }

            var attachedEntities = this.DbContext.ChangeTracker.Entries<TEntity>().Select(e => e.Property("Id").CurrentValue);

            foreach (var item in queryAll)
            {
                var itemId = typeof(TEntity).GetProperty("Id").GetValue(item);
                if (!attachedEntities.Contains(itemId))
                {
                    this.DbContext.Attach(item);
                }
            }

            return queryAll;
        }

        public override TEntity Get<TKey>(TKey id)
        {
            var record = this.Query().FirstOrDefault(c => typeof(TEntity).GetProperty("Id").GetValue(c).Equals(id));

            return record;
        }
    }
}

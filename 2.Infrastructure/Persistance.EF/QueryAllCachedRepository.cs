using Core;
using Core.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Persistance.EF
{
    /// <summary>
    /// Repository with IQueryable for IRepository.Query cached
    /// Should be used on plain entities with small record quantity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class QueryAllCachedRepository<TEntity, TContext> : Repository<TEntity, TContext>
        where TEntity : class, IEntity where TContext : DbContext
    {
        readonly IMemCache _cache;

        public QueryAllCachedRepository(TContext dbContext, 
            IUnitOfWork unitOfWork,
            IMemCache cache) : base(dbContext, unitOfWork)
        {
            _cache = cache;
        }

        public override IQueryable<TEntity> Query()
        {
            var entityName = typeof(TEntity).Name;
            var queryableCacheKey = $"{entityName}_QueryAll";
            var enumCacheValue = (CacheGroup)Enum.Parse(typeof(CacheGroup), entityName);

            _cache.TryGetValue(enumCacheValue, queryableCacheKey, out IQueryable<TEntity> queryAll);

            if (queryAll == null)
            {
                queryAll = base.Query().ToList().AsQueryable();
                _cache.Set(enumCacheValue, queryableCacheKey, queryAll);
            }
            
            var attachedEntities = _dbContext.ChangeTracker.Entries<TEntity>().Select(e => e.Property("Id").CurrentValue);

            foreach (var item in queryAll)
            {
                var itemId = typeof(TEntity).GetProperty("Id").GetValue(item);
                if (!attachedEntities.Contains(itemId))
                    _dbContext.Attach(item);
            }
            return queryAll;
        }

        public override TEntity Get<TKey>(TKey id)
        {
            var record = Query().FirstOrDefault(c => typeof(TEntity).GetProperty("Id").GetValue(c).Equals(id));

            return record;
        }
    }
}

using Core;
using Core.ExtensionHelpers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Services.ExternalServices
{
    public class MicrosoftMemoryCache : IMemCache
    {
        IMemoryCache _cache;

        public MicrosoftMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        private string GenerateCacheKey(CacheGroup group, object key) => string.Format("{0}.{1}", group.GetDescription(), key);

        public TItem Get<TItem>(CacheGroup group, object key) => _cache.Get<TItem>(GenerateCacheKey(group, key));

        public bool TryGetValue<TItem>(CacheGroup group, object key, out TItem value) => _cache.TryGetValue<TItem>(GenerateCacheKey(group, key), out value);

        public void Set(CacheGroup group, object key, object value)
        {
            this.Set(group, key, value, null);
        }

        public void Set(CacheGroup group, object key, object value, ExpirationSettings settings)
        {
            var cacheKey = GenerateCacheKey(group, key);

            if (settings != null)
            {
                var options = new MemoryCacheEntryOptions();

                if (settings.AbsoluteExpiration.HasValue)
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours((double)settings.AbsoluteExpiration.Value);

                if (settings.SlidingExpiration.HasValue)
                    options.SlidingExpiration = TimeSpan.FromHours((double)settings.SlidingExpiration.Value);

                _cache.Set(GenerateCacheKey(group, key), value, options);
            }
            else
            {
                _cache.Set(GenerateCacheKey(group, key), value);
            }
        }

        public void Remove(CacheGroup group)
        {
            foreach (var entry in _cache.GetEntriesByGroup(group))
            {
                _cache.Remove(entry);
            }
        }

        public void Remove(CacheGroup group, object key)
        {
            var cacheKey = GenerateCacheKey(group, key);
            _cache.Remove(cacheKey);
        }
    }

    internal static class MemoryCacheExtension
    {
        public static IEnumerable<object> GetEntriesByGroup(this IMemoryCache memoryCache, CacheGroup group)
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = (IDictionary)memoryCache.GetType().GetField("_entries", flags).GetValue(memoryCache);

            return entries.Keys.Cast<string>().Where(x => x.ToString().StartsWith(group.GetDescription()));
        }
    }
}

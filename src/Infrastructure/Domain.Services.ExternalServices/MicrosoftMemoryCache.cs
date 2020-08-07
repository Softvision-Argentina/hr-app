// <copyright file="MicrosoftMemoryCache.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.ExternalServices
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core;
    using Core.ExtensionHelpers;
    using Microsoft.Extensions.Caching.Memory;

    public class MicrosoftMemoryCache : IMemCache
    {
        private readonly IMemoryCache cache;

        public MicrosoftMemoryCache(IMemoryCache cache)
        {
            this.cache = cache;
        }

        private string GenerateCacheKey(CacheGroup group, object key)
        {
            return string.Format("{0}.{1}", group.GetDescription(), key);
        }

        public TItem Get<TItem>(CacheGroup group, object key)
        {
            return this.cache.Get<TItem>(this.GenerateCacheKey(group, key));
        }

        public bool TryGetValue<TItem>(CacheGroup group, object key, out TItem value)
        {
            return this.cache.TryGetValue<TItem>(this.GenerateCacheKey(group, key), out value);
        }

        public void Set(CacheGroup group, object key, object value)
        {
            this.Set(group, key, value, null);
        }

        public void Set(CacheGroup group, object key, object value, ExpirationSettings settings)
        {
            var cacheKey = this.GenerateCacheKey(group, key);

            if (settings != null)
            {
                var options = new MemoryCacheEntryOptions();

                if (settings.AbsoluteExpiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours((double)settings.AbsoluteExpiration.Value);
                }

                if (settings.SlidingExpiration.HasValue)
                {
                    options.SlidingExpiration = TimeSpan.FromHours((double)settings.SlidingExpiration.Value);
                }

                this.cache.Set(this.GenerateCacheKey(group, key), value, options);
            }
            else
            {
                this.cache.Set(this.GenerateCacheKey(group, key), value);
            }
        }

        public void Remove(CacheGroup group)
        {
            foreach (var entry in this.cache.GetEntriesByGroup(group))
            {
                this.cache.Remove(entry);
            }
        }

        public void Remove(CacheGroup group, object key)
        {
            var cacheKey = this.GenerateCacheKey(group, key);
            this.cache.Remove(cacheKey);
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

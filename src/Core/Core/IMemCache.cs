using System.ComponentModel;

namespace Core
{
    public interface IMemCache
    {
        TItem Get<TItem>(CacheGroup group, object key);
        bool TryGetValue<TItem>(CacheGroup group, object key, out TItem value);
        void Set(CacheGroup group, object key, object value);
        void Set(CacheGroup group, object key, object value, ExpirationSettings settings);
        void Remove(CacheGroup group);
        void Remove(CacheGroup group, object key);
    }

    public enum CacheGroup
    {
        [Description("AMS_CACHE_UR")]
        UserRoles,
        ClientSystem,
        TaskType,
        Assignee
    }

    public enum CacheLevel
    {
        /// <summary>
        /// Expires in 24 hours.
        /// </summary>
        OneDay = 24,
        /// <summary>
        /// Expires in 12 hours.
        /// </summary>
        HalfDay = 12,
        /// <summary>
        /// Expires in 1 hour.
        /// </summary>
        OneHour = 1
    }

    public class ExpirationSettings
    {
        public CacheLevel? AbsoluteExpiration { get; set; }
        public CacheLevel? SlidingExpiration { get; set; }
    }
}

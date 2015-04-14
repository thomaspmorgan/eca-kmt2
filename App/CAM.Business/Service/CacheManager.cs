using System;
using System.Runtime.Caching;

namespace CAM.Business.Service
{
    public static class CacheManager
    {
        private static ObjectCache _cache;
        private static Double _cacheExpirationInMinutes = 10;

        public static Double CacheExpirationInMinutes
        {
            get { return _cacheExpirationInMinutes; }
            set { _cacheExpirationInMinutes = value; }
        }

        public static void Add<T>(T entity, string key) where T : class
        {
            if (_cache == null)
            {
                _cache = MemoryCache.Default;
            }
            if (_cache.Contains(key))
                _cache.Remove(key);
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(CacheExpirationInMinutes);
            _cache.Set(key, entity, cacheItemPolicy);
        }

        public static void Clear(string key)
        {
            if (_cache == null)
            {
                _cache = MemoryCache.Default;
                return;
            }
            _cache.Remove(key);
        }

        public static T Get<T>(string key) where T : class
        {
            if (_cache == null)
            {
                _cache = MemoryCache.Default;
            }
            try
            {
                return (T)_cache.Get(key);
            }
            catch
            {
                return null;
            }
        }
    }
}

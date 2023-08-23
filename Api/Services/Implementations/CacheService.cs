using Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Services.Implementations
{
    public class CacheService: ICacheService
    {
        private IMemoryCache MemoryCache { get; set; }
        public CacheService(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public void Set<T>(string key, T value, MemoryCacheEntryOptions cacheEntryOptions = null)
        {
            if (MemoryCache.Get(key) == null)
            {
                cacheEntryOptions ??= new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(999))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(999));
                MemoryCache.Set(key, value, cacheEntryOptions);
            }
        }

        public T Get<T>(string key)
        {
            var result = MemoryCache.Get<T>(key)
;
            return result;
        }

        public void Clear(string key)
        {
            MemoryCache.Remove(key)
;
        }
    }
}

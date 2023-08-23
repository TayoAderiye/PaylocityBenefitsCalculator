using Microsoft.Extensions.Caching.Memory;

namespace Api.Services.Interfaces
{
    public interface ICacheService
    {
        void Set<T>(string key, T value, MemoryCacheEntryOptions cacheEntryOptions = null);
        T Get<T>(string key);
        void Clear(string key);
    }
}

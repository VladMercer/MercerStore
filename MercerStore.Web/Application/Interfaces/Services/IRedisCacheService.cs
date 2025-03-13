namespace MercerStore.Web.Application.Interfaces
{
    public interface IRedisCacheService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
        Task<T?> GetCacheAsync<T>(string key);
        Task<T> TryGetOrSetCacheAsync<T>(
            string cacheKey,
            Func<Task<T>> factory,
            bool useCache,
            TimeSpan cacheDuration);
    }
}

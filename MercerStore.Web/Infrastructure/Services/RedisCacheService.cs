using MercerStore.Web.Application.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace MercerStore.Web.Infrastructure.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        private readonly JsonSerializerSettings _jsonSettings = new()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.None
        };

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            string json = JsonConvert.SerializeObject(value, _jsonSettings);
            await _cache.StringSetAsync(key, json, expiration);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            string? json = await _cache.StringGetAsync(key);

            if (json is null)
                return default;

            return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
        }

        public async Task<T> TryGetOrSetCacheAsync<T>(
            string cacheKey,
            Func<Task<T>> factory,
            bool useCache,
            TimeSpan cacheDuration)
        {
            if (useCache)
            {
                var cachedData = await GetCacheAsync<T>(cacheKey);

                if (cachedData is not null)
                {
                    return cachedData;
                }
            }

            T result = await factory();

            if (useCache)
            {
                await SetCacheAsync(cacheKey, result, cacheDuration);
            }

            return result;
        }
    }
}
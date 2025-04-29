using MercerStore.Web.Application.Interfaces.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace MercerStore.Web.Infrastructure.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _cache;

    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.None
    };

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _cache = redis.GetDatabase();
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
    {
        var json = JsonConvert.SerializeObject(value, _jsonSettings);
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

            if (cachedData is not null) return cachedData;
        }

        var result = await factory();

        if (useCache) await SetCacheAsync(cacheKey, result, cacheDuration);

        return result;
    }
}

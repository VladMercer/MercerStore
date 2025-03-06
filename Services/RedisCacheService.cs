using MercerStore.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace MercerStore.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            string json = JsonSerializer.Serialize(value, _jsonOptions);
            await _cache.StringSetAsync(key, json, expiration);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            string? json = await _cache.StringGetAsync(key);

            if (json is null)
                return default;

            if (typeof(T) == typeof(string))
            {
                return (T)(object)json;
            }

            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
    }
}

using StackExchange.Redis;
using System.Text.Json;

namespace DevFlow.Projects.Services
{
    public class CacheService
    {
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, int expirySeconds = 60)
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, TimeSpan.FromSeconds(expirySeconds));
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
    }
}

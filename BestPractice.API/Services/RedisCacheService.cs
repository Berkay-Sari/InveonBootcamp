using System.Text.Json;
using StackExchange.Redis;

namespace BestPractice.API.Services;

public class RedisCacheService(IConnectionMultiplexer redisConnection) : IRedisCacheService
{
    private readonly IDatabase _cache = redisConnection.GetDatabase();
    public async Task<T?> GetValueAsync<T>(string key)
    {
        var jsonData = await _cache.StringGetAsync(key);
        if (jsonData.IsNullOrEmpty)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonData.ToString());
    }

    public async Task<bool> SetValueAsync<T>(string key, T value, TimeSpan expiration)
    {
        var jsonData = JsonSerializer.Serialize(value);
        return await _cache.StringSetAsync(key, jsonData, expiration);
    }

    public async Task Clear(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }

    public void ClearAll()
    {
        var redisEndpoints = redisConnection.GetEndPoints(true);
        foreach (var redisEndpoint in redisEndpoints)
        {
            var redisServer = redisConnection.GetServer(redisEndpoint);
            redisServer.FlushAllDatabases();
        }
    }
}
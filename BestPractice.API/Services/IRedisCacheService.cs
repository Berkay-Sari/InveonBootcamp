using BestPractice.API.Models;

namespace BestPractice.API.Services;
public interface IRedisCacheService
{
    Task<T?> GetValueAsync<T>(string key);
    Task<bool> SetValueAsync<T>(string key, T value, TimeSpan expiration);
    Task Clear(string key);
    void ClearAll();
}

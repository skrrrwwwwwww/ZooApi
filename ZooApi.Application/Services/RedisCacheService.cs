using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ZooApi.Application.Interfaces;

namespace ZooApi.Application.Services;

public class RedisCacheService(IDistributedCache? cache) : IRedisCacheService
{
    private readonly JsonSerializerOptions _jsonOptions = new() 
                        { PropertyNameCaseInsensitive = true };
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var value = await cache?.GetStringAsync(key, ct);
        
        return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value, _jsonOptions);
    }

    public async Task SetAsync<T>(string key, 
                                  T value, 
                                  TimeSpan? expiration = null, 
                                  CancellationToken ct = default)
    {
        var options = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expiration 
                                                                           ?? TimeSpan.FromMinutes(5) };
        var jsonData = JsonSerializer.Serialize(value, _jsonOptions);
        cache.SetStringAsync(key, jsonData, options, ct); 
    }

    public async Task RemoveAsync(string key, 
                                  CancellationToken ct = default) => 
        await cache.RemoveAsync(key, ct);
}
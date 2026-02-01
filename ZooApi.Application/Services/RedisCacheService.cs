using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ZooApi.Application.Interfaces;

namespace ZooApi.Application.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisCacheService(IDistributedCache? cache)
    {
        _cache = cache;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var value = await _cache?.GetStringAsync(key, ct);
        
        if (string.IsNullOrEmpty(value)) return default(T);
        
        return JsonSerializer.Deserialize<T>(value, _jsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
        };
        var jsonData = JsonSerializer.Serialize(value, _jsonOptions);
        _cache?.SetStringAsync(key, jsonData, options, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        _cache?.RemoveAsync(key, ct);
    }
}
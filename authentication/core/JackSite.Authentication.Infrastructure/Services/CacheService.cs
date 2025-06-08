using JackSite.Authentication.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using System.Text.Json;
using JackSite.Authentication.Abstractions.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

namespace JackSite.Authentication.Infrastructure.Services;

/// <summary>
/// 混合缓存服务实现
/// </summary>
public class CacheService(HybridCache hybridCache,IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await cache.GetAsync(key);
        if (data == null || data.Length == 0)
        {
            return default;
        }
    
        return JsonSerializer.Deserialize<T>(data);
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        var options = new HybridCacheEntryOptions();
        
        
        await hybridCache.SetAsync(key, value, options);
    }
    
    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
    
    public async Task<bool> ExistsAsync(string key)
    {
        var value = await cache.GetAsync(key);
        return value != null;
    }
    
    public async Task RefreshAsync(string key)
    {
        await cache.RefreshAsync(key);
    }
    
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        // 创建缓存选项
        var options = new HybridCacheEntryOptions();

        // 调用 HybridCache 的 GetOrCreateAsync 方法
        return await hybridCache.GetOrCreateAsync(key, (Func<CancellationToken, ValueTask<T>>)AdaptedFactory, options);

        // 适配工厂方法
        async ValueTask<T> AdaptedFactory(CancellationToken _)
        {
            return await factory();
        }
    }

    public async Task ClearAllAsync()
    {
        if (cache is not RedisCache redisCache) return;

        if (redisCache.GetType()
                .GetField("_connection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(redisCache) is not IConnectionMultiplexer connection) return;
        // 执行FLUSHDB命令清空当前数据库
        var db = connection.GetDatabase();
        await db.ExecuteAsync("FLUSHDB");
    }
}
namespace JackSite.Shared.Caching.Services;

/// <summary>
/// 分布式缓存服务实现
/// </summary>
public class DistributedCacheService(
    IDistributedCache cache,
    ILogger<DistributedCacheService> logger,
    RedisCacheOptions options)
    : ICacheService
{
    /// <summary>
    /// 获取缓存项
    /// </summary>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await cache.GetStringAsync(key, cancellationToken);
            
            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "获取缓存 {Key} 时发生错误", key);
            return default;
        }
    }
    
    /// <summary>
    /// 设置缓存项
    /// </summary>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var options1 = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromSeconds(options.DefaultExpirationSeconds)
            };
            
            var data = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, data, options1, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "设置缓存 {Key} 时发生错误", key);
        }
    }
    
    /// <summary>
    /// 删除缓存项
    /// </summary>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await cache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "删除缓存 {Key} 时发生错误", key);
        }
    }
    
    /// <summary>
    /// 刷新缓存项
    /// </summary>
    public async Task RefreshAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await cache.RefreshAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "刷新缓存 {Key} 时发生错误", key);
        }
    }
    
    /// <summary>
    /// 获取或添加缓存项
    /// </summary>
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var result = await GetAsync<T>(key, cancellationToken);
        
        if (result != null)
            return result;
            
        result = await factory();
        
        await SetAsync(key, result, expiration, cancellationToken);
        
        return result;
    }
}
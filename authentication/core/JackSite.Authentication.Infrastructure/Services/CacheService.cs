namespace JackSite.Authentication.Infrastructure.Services;

/// <summary>
/// 分布式缓存服务实现
/// </summary>
public class CacheService(
    IDistributedCache cache,
    ILogger<CacheService> logger,
    IConnectionMultiplexer? redisConnection = null)
    : ICacheService
{

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var data = await cache.GetStringAsync(key);
            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "从缓存获取 {Key} 时出错", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();

            if (absoluteExpiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = absoluteExpiration.Value;
            }

            if (slidingExpiration.HasValue)
            {
                options.SlidingExpiration = slidingExpiration.Value;
            }

            var data = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, data, options);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "设置缓存 {Key} 时出错", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await cache.RemoveAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "移除缓存 {Key} 时出错", key);
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            return await cache.GetAsync(key) != null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "检查缓存 {Key} 是否存在时出错", key);
            return false;
        }
    }

    public async Task RefreshAsync(string key)
    {
        try
        {
            await cache.RefreshAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "刷新缓存 {Key} 时出错", key);
        }
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        try
        {
            var value = await GetAsync<T>(key);
            if (value != null)
                return value;

            // 缓存中不存在，调用工厂方法创建
            value = await factory();
            
            // 将结果存入缓存
            await SetAsync(key, value, absoluteExpiration, slidingExpiration);
            
            return value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "获取或创建缓存 {Key} 时出错", key);
            // 如果缓存操作失败，直接执行工厂方法
            return await factory();
        }
    }

    public async Task ClearAllAsync()
    {
        try
        {
            if (redisConnection == null)
            {
                logger.LogWarning("Redis 连接不可用，无法清除所有缓存");
                return;
            }

            var db = redisConnection.GetDatabase();
            const string prefix = "auth:";
            var keys = GetAllKeys(prefix);

            foreach (var key in keys)
            {
                await db.KeyDeleteAsync(key);
            }

            logger.LogInformation("已清除所有带前缀 {Prefix} 的缓存", prefix);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "清除所有缓存时出错");
        }
    }

    private List<RedisKey> GetAllKeys(string pattern)
    {
        if (redisConnection == null)
            return [];

        var server = redisConnection.GetServer(redisConnection.GetEndPoints()[0]);

        return server.Keys(pattern: $"{pattern}*").ToList();
    }

    public string BuildCacheKey(params object[] args)
    {
        if (args == null || args.Length == 0)
            throw new ArgumentException("缓存键参数不能为空", nameof(args));

        var sb = new StringBuilder();
        foreach (var arg in args)
        {
            sb.Append($":{arg}");
        }

        // 移除第一个冒号
        return sb.Length > 0 ? sb.ToString().Substring(1) : string.Empty;
    }
}
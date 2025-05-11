using System.Text.Json;
using System.Text.RegularExpressions;
using JackSite.Domain.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace JackSite.Infrastructure.Services;

/// <summary>
/// 混合缓存服务实现
/// </summary>
public class HybridCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogService _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IConnectionMultiplexer? _redisConnection;
    private readonly bool _isRedisAvailable;

    // 默认过期时间
    private static readonly TimeSpan DefaultAbsoluteExpiration = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan DefaultSlidingExpiration = TimeSpan.FromMinutes(10);

    public HybridCacheService(
        IMemoryCache memoryCache,
        IDistributedCache distributedCache,
        ILogService logger,
        IOptions<RedisCacheOptions>? redisOptions = null)
    {
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _logger = logger.ForContext<HybridCacheService>();

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        // 尝试获取Redis连接
        try
        {
            if (redisOptions?.Value.Configuration != null)
            {
                _redisConnection = ConnectionMultiplexer.Connect(redisOptions.Value.Configuration);
                _isRedisAvailable = _redisConnection.IsConnected;

                if (_isRedisAvailable)
                {
                    _logger.Information("Redis连接已建立: {ConnectionString}", 
                        MaskConnectionString(redisOptions.Value.Configuration));
                }
                else
                {
                    _logger.Warning("无法连接到Redis: {ConnectionString}", 
                        MaskConnectionString(redisOptions.Value.Configuration));
                }
            }
            else
            {
                _isRedisAvailable = false;
                _logger.Information("未配置Redis连接，将仅使用内存缓存");
            }
        }
        catch (Exception ex)
        {
            _isRedisAvailable = false;
            _logger.Error(ex, "初始化Redis连接时出错");
        }
    }

    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        // 首先尝试从内存缓存获取
        if (_memoryCache.TryGetValue(key, out T? value))
        {
            _logger.Debug("从内存缓存获取键 {Key}", key);
            return value;
        }

        // 如果内存缓存中不存在，尝试从分布式缓存获取
        var cachedBytes = await _distributedCache.GetAsync(key, cancellationToken);
        if (cachedBytes == null || cachedBytes.Length == 0)
        {
            _logger.Debug("键 {Key} 在缓存中不存在", key);
            return default;
        }

        try
        {
            // 反序列化缓存值
            var cachedValue = JsonSerializer.Deserialize<T>(cachedBytes, _jsonOptions);
            
            // 将值添加到内存缓存
            _memoryCache.Set(key, cachedValue);
            
            _logger.Debug("从分布式缓存获取键 {Key}", key);
            return cachedValue;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "反序列化缓存值时出错，键: {Key}", key);
            return default;
        }
    }

    /// <inheritdoc />
    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        if (value == null)
        {
            _logger.Warning("尝试缓存空值，键: {Key}", key);
            return;
        }

        try
        {
            // 设置内存缓存选项
            var memoryCacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration ?? DefaultAbsoluteExpiration,
                SlidingExpiration = slidingExpiration ?? DefaultSlidingExpiration
            };

            // 设置分布式缓存选项
            var distributedCacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration ?? DefaultAbsoluteExpiration,
                SlidingExpiration = slidingExpiration ?? DefaultSlidingExpiration
            };

            // 序列化值
            var serializedValue = JsonSerializer.SerializeToUtf8Bytes(value, _jsonOptions);

            // 设置内存缓存
            _memoryCache.Set(key, value, memoryCacheOptions);

            // 设置分布式缓存
            await _distributedCache.SetAsync(key, serializedValue, distributedCacheOptions, cancellationToken);

            _logger.Debug("设置缓存键 {Key}，过期时间: {AbsoluteExpiration}, 滑动过期: {SlidingExpiration}", 
                key, 
                absoluteExpiration ?? DefaultAbsoluteExpiration, 
                slidingExpiration ?? DefaultSlidingExpiration);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "设置缓存时出错，键: {Key}", key);
        }
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            // 从内存缓存移除
            _memoryCache.Remove(key);

            // 从分布式缓存移除
            await _distributedCache.RemoveAsync(key, cancellationToken);

            _logger.Debug("移除缓存键 {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "移除缓存时出错，键: {Key}", key);
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        // 首先检查内存缓存
        if (_memoryCache.TryGetValue(key, out _))
        {
            return true;
        }

        // 然后检查分布式缓存
        var cachedBytes = await _distributedCache.GetAsync(key, cancellationToken);
        return cachedBytes is { Length: > 0 };
    }

    /// <inheritdoc />
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        // 尝试获取缓存
        var cachedValue = await GetAsync<T>(key, cancellationToken);
        if (cachedValue != null)
        {
            return cachedValue;
        }

        // 如果缓存不存在，使用工厂方法创建
        _logger.Debug("缓存未命中，使用工厂方法创建值，键: {Key}", key);
        var newValue = await factory();

        // 设置缓存
        await SetAsync(key, newValue, absoluteExpiration, slidingExpiration, cancellationToken);

        return newValue;
    }

    /// <inheritdoc />
    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        // 清除内存缓存
        try
        {
            // 创建一个新的内存缓存实例来替换当前实例
            // 注意：这种方法在实际应用中可能不是最佳选择，因为它会创建一个新的缓存实例
            // 但在当前的IMemoryCache接口中没有提供清除所有缓存的方法
            var fieldInfo = typeof(MemoryCache).GetField("_entries", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (fieldInfo != null)
            {
                var entries = fieldInfo.GetValue(_memoryCache);
                var clearMethod = entries?.GetType().GetMethod("Clear", 
                    BindingFlags.Public | BindingFlags.Instance);
                
                clearMethod?.Invoke(entries, null);
                _logger.Information("内存缓存已清除");
            }
            else
            {
                _logger.Warning("无法访问内存缓存的内部结构，内存缓存可能未完全清除");
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "清除内存缓存时出错");
        }
        
        // 清除Redis缓存
        if (_isRedisAvailable && _redisConnection != null)
        {
            try
            {
                // 获取所有Redis服务器端点
                var endpoints = _redisConnection.GetEndPoints();
                
                foreach (var endpoint in endpoints)
                {
                    var server = _redisConnection.GetServer(endpoint);
                    
                    // 执行FLUSHDB命令清除当前数据库
                    await server.FlushDatabaseAsync();
                    
                    _logger.Information("已清除Redis服务器 {Endpoint} 的当前数据库", endpoint);
                }
                
                _logger.Information("所有Redis缓存已清除");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "清除Redis缓存时出错");
                throw new InvalidOperationException("清除Redis缓存失败", ex);
            }
        }
        else
        {
            _logger.Warning("Redis连接不可用，只清除了内存缓存");
        }
    }

    /// <summary>
    /// 掩盖连接字符串中的敏感信息
    /// </summary>
    private static string MaskConnectionString(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            return string.Empty;
            
        // 简单的掩盖密码的方法，实际应用中可能需要更复杂的处理
        var regex = new Regex("password=([^;,]+)", RegexOptions.IgnoreCase);
            
        return regex.Replace(connectionString, "password=******");
    }
}
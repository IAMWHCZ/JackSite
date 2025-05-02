namespace JackSite.Shared.Caching.Extensions;

/// <summary>
/// 服务集合扩展
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加 Redis 缓存
    /// </summary>
    public static IServiceCollection AddJackSiteRedisCache(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionPath = "Redis")
    {
        // 配置 Redis 选项
        var redisOptions = new RedisCacheOptions();
        configuration.GetSection(configSectionPath).Bind(redisOptions);
        services.AddSingleton(redisOptions);
        
        // 添加 Redis 分布式缓存
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisOptions.ConnectionString;
            options.InstanceName = redisOptions.InstanceName;
        });
        
        // 注册缓存服务
        services.AddSingleton<ICacheService, DistributedCacheService>();
        
        return services;
    }
    
    /// <summary>
    /// 添加内存缓存
    /// </summary>
    public static IServiceCollection AddJackSiteMemoryCache(this IServiceCollection services)
    {
        // 添加内存缓存
        services.AddDistributedMemoryCache();
        
        // 注册缓存服务
        services.AddSingleton(new RedisCacheOptions());
        services.AddSingleton<ICacheService, DistributedCacheService>();
        
        return services;
    }
}
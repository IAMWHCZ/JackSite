using JackSite.Domain.Services;
using JackSite.Infrastructure.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JackSite.Infrastructure.Extensions;

public static class CacheServiceExtensions
{
    public static IServiceCollection AddHybridCache(this IServiceCollection services, IConfiguration configuration)
    {
        // 添加内存缓存
        services.AddMemoryCache();
        
        // 获取Redis连接字符串
        var redisConnectionString = configuration.GetConnectionString("Redis");
        
        // 配置Redis缓存
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            // 添加Redis分布式缓存
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "JackSite:";
            });
            
            // 注册Redis选项，以便HybridCacheService可以访问连接字符串
            services.Configure<RedisCacheOptions>(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "JackSite:";
            });
        }
        else
        {
            // 如果没有配置Redis，使用内存分布式缓存作为后备
            services.AddDistributedMemoryCache();
        }
        
        // 注册混合缓存服务
        services.AddScoped<ICacheService, HybridCacheService>();
        
        return services;
    }
}
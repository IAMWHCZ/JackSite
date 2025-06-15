using JackSite.Authentication.Abstractions.Services;
using JackSite.Authentication.Infrastructure.Services;
using JackSite.Authentication.WebAPI.Modules;
using JackSite.Authentication.WebAPI.Providers;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace JackSite.Authentication.WebAPI.Extensions;

public static class ApiModuleExtensions
{
    public static IServiceCollection AddApiModules(this IServiceCollection services)
    {
        // 自动扫描并注册所有IApiModule的实现
        var moduleTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IApiModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var moduleType in moduleTypes)
        {
            services.AddSingleton(typeof(IApiModule), moduleType);
        }

        return services;
    }

    // 在 ServiceCollectionExtensions.cs 中修改 AddClientCors 方法

    public static IServiceCollection AddClientCors(this IServiceCollection services)
    {
        // 添加内存缓存
        services.AddMemoryCache();
    
        // 注册 CORS 缓存服务
        services.AddScoped<ICorsOriginCacheService, CorsOriginCacheService>();
    
        // 添加 CORS 服务
        services.AddCors(options =>
        {
            options.AddPolicy("DynamicCorsPolicy", policy =>
            {
                // 这里的配置会被 DynamicCorsPolicyProvider 覆盖
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        
        services.AddScoped<ICorsPolicyProvider, DynamicCorsPolicyProvider>();
    
        return services;
    }
    
}
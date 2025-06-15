using JackSite.Authentication.Abstractions.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace JackSite.Authentication.WebAPI.Providers;

public class DynamicCorsPolicyProvider(ICorsOriginCacheService service):ICorsPolicyProvider
{
    public Task<CorsPolicy?> GetPolicyAsync(HttpContext context, string? policyName)
    {
        // 获取允许的源
        var allowedOrigins = service.GetAllowedOrigins();
        
        // 创建并配置策略
        var builder = new CorsPolicyBuilder();
        
        if (allowedOrigins.Count != 0)
        {
            builder.WithOrigins(allowedOrigins.ToArray());
        }
        
        var policy = builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
            .Build();
            
        return Task.FromResult<CorsPolicy?>(policy);
    }
}
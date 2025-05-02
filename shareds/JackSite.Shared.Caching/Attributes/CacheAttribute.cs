using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JackSite.Shared.Caching.Attributes;

/// <summary>
/// 缓存特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute(int timeToLiveSeconds = 60, string keyPrefix = "") : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 获取缓存服务
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
        
        // 生成缓存键
        var cacheKey = GenerateCacheKey(context);
        
        // 尝试从缓存获取
        var cachedResponse = await cacheService.GetAsync<object>(cacheKey);
        
        if (cachedResponse != null)
        {
            context.Result = new OkObjectResult(cachedResponse);
            return;
        }
        
        // 执行请求
        var executedContext = await next();
        
        // 如果结果有效，则缓存
        if (executedContext.Result is ObjectResult { Value: not null } objectResult)
        {
            await cacheService.SetAsync(
                cacheKey,
                objectResult.Value,
                TimeSpan.FromSeconds(timeToLiveSeconds));
        }
    }
    
    private string GenerateCacheKey(ActionExecutingContext context)
    {
        var prefix = string.IsNullOrEmpty(keyPrefix)
            ? $"{context.Controller.GetType().Name}:{context.ActionDescriptor.DisplayName}"
            : keyPrefix;
            
        var parameters = context.ActionArguments.Values.ToArray();
        
        return CacheKeyGenerator.Generate(prefix, parameters!);
    }
}
using Microsoft.AspNetCore.Http;

namespace JackSite.Shared.Caching.Middleware;

/// <summary>
/// 缓存中间件
/// </summary>
public class CachingMiddleware(
    RequestDelegate next,
    ILogger<CachingMiddleware> logger,
    RedisCacheOptions options)
{
    public async Task InvokeAsync(HttpContext context, ICacheService cacheService)
    {
        // 只缓存 GET 请求
        if (context.Request.Method != HttpMethod.Get.Method)
        {
            await next(context);
            return;
        }
        
        // 生成缓存键
        var cacheKey = $"API:{context.Request.Path}{context.Request.QueryString}";
        
        // 尝试从缓存获取
        var cachedResponse = await cacheService.GetAsync<string>(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedResponse))
        {
            logger.LogDebug("从缓存返回: {CacheKey}", cacheKey);
            
            context.Response.ContentType = "application/json";
            context.Response.Headers.Append("X-Cache", "HIT");
            await context.Response.WriteAsync(cachedResponse);
            return;
        }
        
        // 保存原始响应体流
        var originalBodyStream = context.Response.Body;
        
        try
        {
            // 创建新的内存流
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            
            // 继续处理请求
            await next(context);
            
            // 如果响应成功且是 JSON
            if (context.Response.StatusCode == 200 && 
                context.Response.ContentType?.Contains("application/json") == true)
            {
                // 读取响应
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseContent = await new StreamReader(responseBody).ReadToEndAsync();
                
                // 缓存响应
                await cacheService.SetAsync(
                    cacheKey,
                    responseContent,
                    TimeSpan.FromSeconds(options.DefaultExpirationSeconds));
                    
                logger.LogDebug("缓存响应: {CacheKey}", cacheKey);
                
                // 重置流位置
                responseBody.Seek(0, SeekOrigin.Begin);
            }
            
            // 复制到原始流
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            // 恢复原始响应体流
            context.Response.Body = originalBodyStream;
        }
    }
}
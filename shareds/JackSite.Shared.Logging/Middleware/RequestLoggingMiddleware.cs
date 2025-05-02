
namespace JackSite.Shared.Logging.Middleware;

/// <summary>
/// 请求日志中间件
/// </summary>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        
        // 记录请求开始
        logger.LogInformation(
            "开始处理请求 {Method} {Path}{QueryString}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString);
            
        try
        {
            await next(context);
            sw.Stop();
            
            // 记录请求完成
            logger.LogInformation(
                "完成请求 {Method} {Path}{QueryString} - 状态码: {StatusCode}, 耗时: {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            sw.Stop();
            
            // 记录请求异常
            logger.LogError(
                ex,
                "处理请求 {Method} {Path}{QueryString} 时发生错误, 耗时: {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                sw.ElapsedMilliseconds);
                
            throw;
        }
    }
}
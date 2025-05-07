using JackSite.Infrastructure.Logging;

namespace JackSite.Http.Middleware;

/// <summary>
/// 日志上下文中间件
/// </summary>
public class LogContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // 添加请求上下文信息到日志
        using (LoggingExtensions.EnrichWithRequestContext(
            context.Request.Path,
            context.Request.Method,
            context.Connection.RemoteIpAddress?.ToString(),
            context.User.Identity?.IsAuthenticated == true ? context.User.FindFirst("UserId")?.Value : null,
            context.User.Identity?.IsAuthenticated == true ? context.User.FindFirst("UserName")?.Value : null))
        {
            await next(context);
        }
    }
}

/// <summary>
/// 日志上下文中间件扩展方法
/// </summary>
public static class LogContextMiddlewareExtensions
{
    /// <summary>
    /// 使用日志上下文中间件
    /// </summary>
    /// <param name="app">应用程序构建器</param>
    /// <returns>应用程序构建器</returns>
    public static IApplicationBuilder UseLogContext(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogContextMiddleware>();
    }
}
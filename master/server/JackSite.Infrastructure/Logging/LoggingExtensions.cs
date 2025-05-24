using Serilog.Context;

namespace JackSite.Infrastructure.Logging;

/// <summary>
/// 日志扩展方法
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// 添加请求上下文信息到日志
    /// </summary>
    /// <param name="requestPath">请求路径</param>
    /// <param name="requestMethod">请求方法</param>
    /// <param name="clientIp">客户端IP</param>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    /// <returns>可释放对象，用于清理日志上下文</returns>
    public static IDisposable EnrichWithRequestContext(
        string requestPath,
        string requestMethod,
        string? clientIp = null,
        string? userId = null,
        string? userName = null)
    {
        var disposables = new List<IDisposable>(5)
        {
            LogContext.PushProperty("RequestPath", requestPath),
            LogContext.PushProperty("RequestMethod", requestMethod)
        };

        if (!string.IsNullOrEmpty(clientIp))
            disposables.Add(LogContext.PushProperty("ClientIp", clientIp));
        
        if (!string.IsNullOrEmpty(userId))
            disposables.Add(LogContext.PushProperty("UserId", userId));
        
        if (!string.IsNullOrEmpty(userName))
            disposables.Add(LogContext.PushProperty("UserName", userName));
        
        return new CompositeDisposable(disposables);
    }

    /// <summary>
    /// 添加用户上下文信息到日志
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    /// <returns>可释放对象，用于清理日志上下文</returns>
    public static IDisposable EnrichWithUserContext(string userId, string? userName = null)
    {
        var disposables = new List<IDisposable>(2) { LogContext.PushProperty("UserId", userId) };

        if (!string.IsNullOrEmpty(userName))
            disposables.Add(LogContext.PushProperty("UserName", userName));
        
        return new CompositeDisposable(disposables);
    }

    /// <summary>
    /// 添加自定义属性到日志上下文
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <param name="propertyValue">属性值</param>
    /// <returns>可释放对象，用于清理日志上下文</returns>
    public static IDisposable EnrichWithProperty(string propertyName, object? propertyValue)
    {
        return LogContext.PushProperty(propertyName, propertyValue);
    }

    /// <summary>
    /// 添加多个自定义属性到日志上下文
    /// </summary>
    /// <param name="properties">属性字典</param>
    /// <returns>可释放对象，用于清理日志上下文</returns>
    public static IDisposable EnrichWithProperties(IDictionary<string, object?> properties)
    {
        var disposables = properties.Select(p => LogContext.PushProperty(p.Key, p.Value)).ToList();
        return new CompositeDisposable(disposables);
    }

    private class CompositeDisposable(IEnumerable<IDisposable> disposables) : IDisposable
    {
        private readonly IReadOnlyList<IDisposable> _disposables = disposables.ToList();

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
using System.Diagnostics;

namespace JackSite.Authentication.Application.Behaviors;

/// <summary>
/// 性能监控行为
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        _timer.Start();
        
        var response = await next(cancellationToken);
        
        _timer.Stop();
        
        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        
        if (elapsedMilliseconds > 500)
        {
            // 如果请求处理时间超过500毫秒，记录警告日志
            logger.LogWarning("Long running request: {RequestName} ({ElapsedMilliseconds} milliseconds)",
                requestName, elapsedMilliseconds);
        }
        else
        {
            logger.LogDebug("Request {RequestName} completed in {ElapsedMilliseconds} ms", 
                requestName, elapsedMilliseconds);
        }
        
        return response;
    }
}
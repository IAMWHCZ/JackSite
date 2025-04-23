namespace JackSite.Infrastructure.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500) return response; // 如果处理时间超过500ms，记录警告
        var requestName = typeof(TRequest).Name;
            
        logger.LogWarning("Long Running Request: {RequestName} ({ElapsedMilliseconds} milliseconds)",
            requestName, elapsedMilliseconds);

        return response;
    }
}
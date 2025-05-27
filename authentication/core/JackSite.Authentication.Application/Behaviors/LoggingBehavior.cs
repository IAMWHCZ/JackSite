namespace JackSite.Authentication.Application.Behaviors;

/// <summary>
/// 请求日志记录行为
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid().ToString();

        logger.LogInformation("[{RequestId}] Handling request {RequestName}", requestId, requestName);

        try
        {
            var response = await next(cancellationToken);
            logger.LogInformation("[{RequestId}] Request {RequestName} handled successfully", requestId, requestName);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{RequestId}] Request {RequestName} processing failed", requestId, requestName);
            throw;
        }
    }
}
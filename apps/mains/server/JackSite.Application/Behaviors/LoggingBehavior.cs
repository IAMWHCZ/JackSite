using System.Diagnostics;
using System.Text.Json;
using JackSite.Domain.Services;

namespace JackSite.Application.Behaviors;

/// <summary>
/// MediatR 请求日志行为
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogService logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogService _logger = logger.ForContext<LoggingBehavior<TRequest, TResponse>>();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    // 配置 JSON 序列化选项

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        var requestName = requestType.Name;
        var requestId = Guid.NewGuid().ToString();
        var requestNamespace = requestType.Namespace;
        var requestAssembly = requestType.Assembly.GetName().Name;
        
        // 序列化请求对象，但限制大小以避免日志过大
        string requestJson;
        try
        {
            requestJson = JsonSerializer.Serialize(request, _jsonOptions);
            if (requestJson.Length > 10000) // 限制为10KB
            {
                requestJson = requestJson.Substring(0, 10000) + "... [截断]";
            }
        }
        catch (Exception ex)
        {
            requestJson = $"无法序列化请求: {ex.Message}";
        }
        
        // 创建带有丰富上下文的日志记录器
        var contextLogger = _logger
            .ForContext("RequestId", requestId)
            .ForContext("RequestType", requestName)
            .ForContext("RequestNamespace", requestNamespace)
            .ForContext("RequestAssembly", requestAssembly)
            .ForContext("RequestData", requestJson)
            .ForContext("RequestTime", DateTime.UtcNow);
        
        contextLogger.Information("开始处理请求 {RequestName}", requestName);
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // 执行请求处理
            var response = await next(cancellationToken);
            
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            // 尝试序列化响应（如果不是太大）
            string? responseJson = null;
            if (response != null && !(response is Stream) && !(response is Task))
            {
                try
                {
                    responseJson = JsonSerializer.Serialize(response, _jsonOptions);
                    if (responseJson.Length > 10000) // 限制为10KB
                    {
                        responseJson = responseJson.Substring(0, 10000) + "... [截断]";
                    }
                }
                catch
                {
                    responseJson = "无法序列化响应";
                }
            }
            
            // 记录成功处理的请求
            contextLogger
                .ForContext("ElapsedMilliseconds", elapsedMs)
                .ForContext("ResponseData", responseJson)
                .Information("成功处理请求 {RequestName}，耗时 {ElapsedMilliseconds}ms", 
                    requestName, elapsedMs);
            
            // 如果请求处理时间过长，记录警告
            if (elapsedMs > 500)
            {
                contextLogger.Warning("请求 {RequestName} 处理时间过长: {ElapsedMilliseconds}ms", 
                    requestName, elapsedMs);
            }
            
            return response;
        }
        catch (Exception ex)
        {
            // 记录请求处理失败
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            contextLogger
                .ForContext("ElapsedMilliseconds", elapsedMs)
                .ForContext("ExceptionType", ex.GetType().Name)
                .ForContext("ExceptionStackTrace", ex.StackTrace)
                .Error(ex, "处理请求 {RequestName} 失败，耗时 {ElapsedMilliseconds}ms: {ErrorMessage}", 
                    requestName, elapsedMs, ex.Message);
            
            // 记录内部异常（如果有）
            if (ex.InnerException != null)
            {
                contextLogger.Error(ex.InnerException, "请求 {RequestName} 的内部异常: {ErrorMessage}", 
                    requestName, ex.InnerException.Message);
            }
            
            throw;
        }
    }
}
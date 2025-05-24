using System.Diagnostics;
using System.Text.Json;
using JackSite.Domain.Services;

namespace JackSite.Application.Behaviors;

/// <summary>
/// MediatR 请求性能监控行为
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class PerformanceBehavior<TRequest, TResponse>(ILogService logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogService _logger = logger.ForContext<PerformanceBehavior<TRequest, TResponse>>();
    private readonly Stopwatch _timer = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    
    // 性能警告阈值（毫秒）
    private const int WarningThresholdMs = 500;
    
    // 性能严重警告阈值（毫秒）
    private const int CriticalThresholdMs = 2000;
    
    // 性能统计阈值（毫秒）- 超过此阈值的请求将被记录用于统计
    private const int StatisticsThresholdMs = 100;

    // 配置 JSON 序列化选项

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        var requestName = requestType.Name;
        var requestNamespace = requestType.Namespace;
        
        // 创建性能监控上下文
        var perfLogger = _logger
            .ForContext("RequestType", requestName)
            .ForContext("RequestNamespace", requestNamespace)
            .ForContext("PerformanceMonitor", true);
        
        _timer.Start();
        
        // 记录内存使用情况（开始）
        var startMemory = GC.GetTotalMemory(false);
        
        // 记录线程信息
        var threadId = Environment.CurrentManagedThreadId;
        var isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread;
        
        try
        {
            // 执行请求处理
            var response = await next(cancellationToken);
            
            _timer.Stop();
            
            // 计算性能指标
            var elapsedMs = _timer.ElapsedMilliseconds;
            var endMemory = GC.GetTotalMemory(false);
            var memoryDelta = endMemory - startMemory;

            switch (elapsedMs)
            {
                // 根据执行时间记录不同级别的日志
                case > CriticalThresholdMs:
                {
                    // 严重性能问题
                    var requestJson = TrySerializeRequest(request);
                
                    perfLogger
                        .ForContext("ElapsedMilliseconds", elapsedMs)
                        .ForContext("MemoryUsedBytes", memoryDelta)
                        .ForContext("ThreadId", threadId)
                        .ForContext("IsThreadPoolThread", isThreadPoolThread)
                        .ForContext("RequestData", requestJson)
                        .ForContext("Severity", "Critical")
                        .Warning("严重性能问题: 请求 {RequestName} 执行时间 {ElapsedMilliseconds}ms，内存使用 {MemoryUsedMB:F2}MB", 
                            requestName, elapsedMs, memoryDelta / (1024.0 * 1024.0));
                    break;
                }
                case > WarningThresholdMs:
                {
                    // 性能警告
                    string requestJson = TrySerializeRequest(request);
                
                    perfLogger
                        .ForContext("ElapsedMilliseconds", elapsedMs)
                        .ForContext("MemoryUsedBytes", memoryDelta)
                        .ForContext("ThreadId", threadId)
                        .ForContext("IsThreadPoolThread", isThreadPoolThread)
                        .ForContext("RequestData", requestJson)
                        .ForContext("Severity", "Warning")
                        .Warning("性能警告: 请求 {RequestName} 执行时间 {ElapsedMilliseconds}ms，内存使用 {MemoryUsedMB:F2}MB", 
                            requestName, elapsedMs, memoryDelta / (1024.0 * 1024.0));
                    break;
                }
                case > StatisticsThresholdMs:
                    // 记录用于统计的性能数据
                    perfLogger
                        .ForContext("ElapsedMilliseconds", elapsedMs)
                        .ForContext("MemoryUsedBytes", memoryDelta)
                        .ForContext("Severity", "Statistics")
                        .Information("性能统计: 请求 {RequestName} 执行时间 {ElapsedMilliseconds}ms", 
                            requestName, elapsedMs);
                    break;
            }
            
            return response;
        }
        catch (Exception ex)
        {
            _timer.Stop();
            
            var elapsedMs = _timer.ElapsedMilliseconds;
            var endMemory = GC.GetTotalMemory(false);
            var memoryDelta = endMemory - startMemory;
            
            // 记录异常情况下的性能数据
            perfLogger
                .ForContext("ElapsedMilliseconds", elapsedMs)
                .ForContext("MemoryUsedBytes", memoryDelta)
                .ForContext("ThreadId", threadId)
                .ForContext("IsThreadPoolThread", isThreadPoolThread)
                .ForContext("ExceptionType", ex.GetType().Name)
                .ForContext("Severity", "Error")
                .Error(ex, "性能异常: 请求 {RequestName} 在执行 {ElapsedMilliseconds}ms 后失败，内存使用 {MemoryUsedMB:F2}MB", 
                    requestName, elapsedMs, memoryDelta / (1024.0 * 1024.0));
            
            throw; // 重新抛出异常以保持原始行为
        }
        finally
        {
            // 确保计时器停止
            if (_timer.IsRunning)
            {
                _timer.Stop();
            }
            
            // 建议进行垃圾回收（仅在严重内存使用情况下）
            if (GC.GetTotalMemory(false) - startMemory > 100 * 1024 * 1024) // 超过100MB
            {
                GC.Collect();
            }
        }
    }
    
    /// <summary>
    /// 尝试序列化请求对象，处理可能的异常
    /// </summary>
    private string TrySerializeRequest(TRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            if (json.Length > 8000) // 限制为8KB
            {
                return json[..8000] + "... [截断]";
            }
            return json;
        }
        catch (Exception ex)
        {
            return $"无法序列化请求: {ex.Message}";
        }
    }
}
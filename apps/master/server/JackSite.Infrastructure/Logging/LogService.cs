using JackSite.Domain.Services;
using Serilog.Context;
using Serilog.Events;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace JackSite.Infrastructure.Logging;

/// <summary>
/// Serilog 日志服务实现
/// </summary>
public class LogService : ILogService
{
    private readonly ILogger _logger;
    private readonly IBaseRepository<LogEntry,long>? _logRepository;

    /// <summary>
    /// 创建日志服务实例
    /// </summary>
    public LogService(IBaseRepository<LogEntry,long>? logRepository = null)
    {
        _logger = Log.Logger;
        _logRepository = logRepository;
    }

    /// <summary>
    /// 使用指定的 Serilog 日志记录器创建日志服务实例
    /// </summary>
    private LogService(ILogger logger, IBaseRepository<LogEntry,long>? logRepository = null)
    {
        _logger = logger;
        _logRepository = logRepository;
    }

    /// <summary>
    /// 记录调试信息
    /// </summary>
    public void Debug(string message, params object[] propertyValues)
    {
        _logger.Debug(message, propertyValues);
        SaveToDatabase(LogEventLevel.Debug, message, null, propertyValues);
    }

    /// <summary>
    /// 记录普通信息
    /// </summary>
    public void Information(string message, params object[] propertyValues)
    {
        _logger.Information(message, propertyValues);
        SaveToDatabase(LogEventLevel.Information, message, null, propertyValues);
    }

    /// <summary>
    /// 记录警告信息
    /// </summary>
    public void Warning(string message, params object[] propertyValues)
    {
        _logger.Warning(message, propertyValues);
        SaveToDatabase(LogEventLevel.Warning, message, null, propertyValues);
    }

    /// <summary>
    /// 记录错误信息
    /// </summary>
    public void Error(Exception? exception, string message, params object[] propertyValues)
    {
        _logger.Error(exception, message, propertyValues);
        SaveToDatabase(LogEventLevel.Error, message, exception, propertyValues);
    }

    /// <summary>
    /// 记录错误信息（无异常）
    /// </summary>
    public void Error(string message, params object[] propertyValues)
    {
        _logger.Error(message, propertyValues);
        SaveToDatabase(LogEventLevel.Error, message, null, propertyValues);
    }

    /// <summary>
    /// 记录致命错误
    /// </summary>
    public void Fatal(Exception? exception, string message, params object[] propertyValues)
    {
        _logger.Fatal(exception, message, propertyValues);
        SaveToDatabase(LogEventLevel.Fatal, message, exception, propertyValues);
    }

    /// <summary>
    /// 记录致命错误（无异常）
    /// </summary>
    public void Fatal(string message, params object[] propertyValues)
    {
        _logger.Fatal(message, propertyValues);
        SaveToDatabase(LogEventLevel.Fatal, message, null, propertyValues);
    }

    /// <summary>
    /// 获取指定类型的日志记录器
    /// </summary>
    public ILogService ForContext<T>()
    {
        return new LogService(_logger.ForContext<T>(), _logRepository);
    }

    /// <summary>
    /// 获取指定类型的日志记录器
    /// </summary>
    public ILogService ForContext(Type type)
    {
        return new LogService(_logger.ForContext(type), _logRepository);
    }

    /// <summary>
    /// 获取指定上下文的日志记录器
    /// </summary>
    public ILogService ForContext(string propertyName, object? value, bool destructureObjects = false)
    {
        return new LogService(_logger.ForContext(propertyName, value, destructureObjects), _logRepository);
    }

    /// <summary>
    /// 创建一个带有操作上下文的日志服务
    /// </summary>
    public static ILogService ForOperation(string operationName, IBaseRepository<LogEntry,long>? logRepository = null)
    {
        return new LogService(Log.Logger.ForContext("Operation", operationName), logRepository);
    }

    /// <summary>
    /// 创建一个带有用户上下文的日志服务
    /// </summary>
    public static ILogService ForUser(long userId, string? userName = null,
        IBaseRepository<LogEntry,long>? logRepository = null)
    {
        var logger = Log.Logger.ForContext("UserId", userId);

        if (!string.IsNullOrEmpty(userName))
        {
            logger = logger.ForContext("UserName", userName);
        }

        return new LogService(logger, logRepository);
    }

    /// <summary>
    /// 创建一个带有请求上下文的日志服务
    /// </summary>
    public static ILogService ForRequest(string path, string method, string? clientIp = null,
        IBaseRepository<LogEntry,long>? logRepository = null)
    {
        var logger = Log.Logger
            .ForContext("RequestPath", path)
            .ForContext("RequestMethod", method);

        if (!string.IsNullOrEmpty(clientIp))
        {
            logger = logger.ForContext("ClientIp", clientIp);
        }

        return new LogService(logger, logRepository);
    }

    /// <summary>
    /// 使用日志上下文执行操作
    /// </summary>
    public static IDisposable PushProperty(string name, object? value, bool destructureObjects = false)
    {
        return LogContext.PushProperty(name, value, destructureObjects);
    }

    /// <summary>
    /// 将日志保存到数据库
    /// </summary>
    private void SaveToDatabase(LogEventLevel level, string message, Exception? exception, object[] propertyValues)
    {
        if (_logRepository == null) return;

        try
        {
            // 创建日志条目
            var logEntry = new LogEntry
            {
                Level = (Domain.Enums.LogEventLevel)level,
                Message = message,
                Exception = exception?.ToString(),
                Timestamp = DateTime.UtcNow,
                Source = GetSourceContext()
            };

            // 提取用户ID和用户名
            var userId = GetPropertyValue<long?>("UserId");

            if (userId.HasValue)
            {
                logEntry.UpdateBy = userId.Value;
            }

            // 提取请求信息
            logEntry.RequestPath = GetPropertyValue<string>("RequestPath");
            logEntry.RequestMethod = GetPropertyValue<string>("RequestMethod");
            logEntry.ClientIp = GetPropertyValue<string>("ClientIp");

            // 序列化属性
            var properties = new Dictionary<string, object?>();

            // 添加结构化属性
            for (int i = 0; i < propertyValues.Length; i += 2)
            {
                if (i + 1 < propertyValues.Length && propertyValues[i] is string key)
                {
                    properties[key] = propertyValues[i + 1];
                }
            }

            // 序列化为JSON
            if (properties.Count > 0)
            {
                logEntry.Properties = JsonSerializer.Serialize(properties);
            }

            // 异步保存到数据库
            Task.Run(async () => await _logRepository.AddAsync(logEntry));
        }
        catch
        {
            // 忽略数据库保存错误，避免影响应用程序
        }
    }

    /// <summary>
    /// 获取源上下文
    /// </summary>
    private string? GetSourceContext()
    {
        return GetPropertyValue<string>("SourceContext");
    }

    /// <summary>
    /// 从日志上下文中获取属性值
    /// </summary>
    private static T? GetPropertyValue<T>(string propertyName)
    {
        try
        {
            // 尝试从Serilog上下文中获取属性值
            // 注意：这是一个简化实现，实际上Serilog不直接提供此功能
            // 在实际应用中，可能需要通过自定义Sink或其他方式实现
            return default;
        }
        catch
        {
            return default;
        }
    }
}
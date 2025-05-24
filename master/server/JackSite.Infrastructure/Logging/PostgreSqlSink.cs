using System.Text.Json;
using Serilog.Core;
using Serilog.Events;
using LogEventLevel = JackSite.Domain.Enums.LogEventLevel;

namespace JackSite.Infrastructure.Logging;

/// <summary>
/// 自定义 PostgreSQL 日志接收器
/// </summary>
public class PostgreSqlSink : ILogEventSink
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _machineName;

    /// <summary>
    /// 初始化 PostgreSQL 日志接收器
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    public PostgreSqlSink(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _machineName = Environment.MachineName;
    }

    /// <summary>
    /// 处理日志事件
    /// </summary>
    /// <param name="logEvent">日志事件</param>
    public void Emit(LogEvent logEvent)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            
            // 如果数据库上下文不可用，则跳过保存到数据库
            if (dbContext == null)
            {
                return;
            }
            
            var logEntry = CreateLogEntry(logEvent);
            dbContext.Logs.Add(logEntry);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            // 记录失败不应该影响应用程序运行
            Console.WriteLine($"Failed to save log to database: {ex.Message}");
        }
    }

    /// <summary>
    /// 创建日志条目
    /// </summary>
    /// <param name="logEvent">日志事件</param>
    /// <returns>日志条目</returns>
    private LogEntry CreateLogEntry(LogEvent logEvent)
    {
        var properties = logEvent.Properties;
        var logEntry = new LogEntry
        {
            Level = (LogEventLevel)logEvent.Level,
            Timestamp = logEvent.Timestamp.UtcDateTime,
            Message = logEvent.RenderMessage(),
            Exception = logEvent.Exception?.ToString(),
            Source = properties.TryGetValue("SourceContext", out var sourceContext)
                ? sourceContext.ToString().Trim('"')
                : null,
            Properties = SerializeProperties(properties),
            MachineName = _machineName,
            
            // 从日志属性中提取请求和用户信息
            RequestPath = GetPropertyValue(properties, "RequestPath"),
            RequestMethod = GetPropertyValue(properties, "RequestMethod"),
            ClientIp = GetPropertyValue(properties, "ClientIp"),
            UpdateBy = GetPropertyValue(properties, "UserId") is { } userId && long.TryParse(userId, out var id) 
                ? id 
                : 0
        };

        return logEntry;
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="properties">属性字典</param>
    /// <param name="propertyName">属性名</param>
    /// <returns>属性值</returns>
    private static string? GetPropertyValue(
        IReadOnlyDictionary<string, LogEventPropertyValue> properties, 
        string propertyName)
    {
        return properties.TryGetValue(propertyName, out var value) 
            ? value.ToString().Trim('"') 
            : null;
    }

    /// <summary>
    /// 序列化属性
    /// </summary>
    /// <param name="properties">属性字典</param>
    /// <returns>序列化后的JSON字符串</returns>
    private static string? SerializeProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
    {
        try
        {
            var dict = properties.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.ToString().Trim('"'));

            return JsonSerializer.Serialize(dict);
        }
        catch
        {
            return null;
        }
    }
}
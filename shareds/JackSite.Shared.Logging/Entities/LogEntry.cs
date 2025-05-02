using JackSite.Shared.EntityFrameworkCore.Entities;

namespace JackSite.Shared.Logging.Entities;

/// <summary>
/// 自定义日志实体
/// </summary>
public class LogEntry : AuditableEntityBase<int>
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public string Level { get; set; } = null!;
    
    /// <summary>
    /// 日志消息
    /// </summary>
    public string Message { get; set; } = null!;
    
    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Exception { get; set; }
    
    /// <summary>
    /// 日志来源
    /// </summary>
    public string? Source { get; set; }
    
    /// <summary>
    /// 跟踪ID
    /// </summary>
    public string? TraceId { get; set; }
    
    /// <summary>
    /// 应用名称
    /// </summary>
    public string? Application { get; set; }
    
    /// <summary>
    /// 附加属性 (JSON格式)
    /// </summary>
    public string? Properties { get; set; }
}
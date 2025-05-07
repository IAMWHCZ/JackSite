using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JackSite.Domain.Entities;

/// <summary>
/// 系统日志实体
/// </summary>
public class LogEntry
{
    /// <summary>
    /// 日志ID
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    [Required]
    [MaxLength(10)]
    [Column(TypeName = "varchar(10)")]
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// 日志时间戳
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 日志来源
    /// </summary>
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? Source { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    [Required]
    [Column(TypeName = "text")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 异常信息
    /// </summary>
    [Column(TypeName = "text")]
    public string? Exception { get; set; }

    /// <summary>
    /// 日志上下文（JSON格式）
    /// </summary>
    [Column(TypeName = "jsonb")]
    public string? Properties { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    [MaxLength(500)]
    [Column(TypeName = "varchar(500)")]
    public string? RequestPath { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    [MaxLength(10)]
    [Column(TypeName = "varchar(10)")]
    public string? RequestMethod { get; set; }

    /// <summary>
    /// 客户端IP
    /// </summary>
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? ClientIp { get; set; }

    /// <summary>
    /// 用户ID（如果已登录）
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名（如果已登录）
    /// </summary>
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? UserName { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? ApplicationName { get; set; }

    /// <summary>
    /// 机器名称
    /// </summary>
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? MachineName { get; set; }
}
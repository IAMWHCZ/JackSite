using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JackSite.Notification.Grpc.Enums;
using JackSite.Shared.Core.IdGenerator;
using JackSite.Shared.EntityFrameworkCore.Entities;

namespace JackSite.Notification.Grpc.Entities;

/// <summary>
/// 邮件发送记录
/// </summary>
public class EmailLog:AuditableEntityBase<SnowflakeId>
{
    
    /// <summary>
    /// 收件人邮箱
    /// </summary>
    [Required]
    [Column("to_email")]
    [StringLength(255)]
    public string ToEmail { get; set; } = null!;
    
    /// <summary>
    /// 邮件主题
    /// </summary>
    [Required]
    [Column("subject")]
    [StringLength(255)]
    public string Subject { get; set; } = null!;
    
    /// <summary>
    /// 邮件类型
    /// </summary>
    [Required]
    [Column("email_type")]
    [StringLength(50)]
    public string EmailType { get; set; } = null!;
    
    /// <summary>
    /// 发送状态
    /// </summary>
    [Required]
    [Column("status")]
    public EmailStatus Status { get; set; }
    
    /// <summary>
    /// 错误信息
    /// </summary>
    [Column("error_message")]
    [StringLength(1000)]
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// 相关用户ID
    /// </summary>
    [Column("user_id")]
    [StringLength(100)]
    public string? UserId { get; set; }
    
    /// <summary>
    /// 相关用户名
    /// </summary>
    [Column("username")]
    [StringLength(100)]
    public string? Username { get; set; }
    
    /// <summary>
    /// 验证令牌
    /// </summary>
    [Column("verification_token")]
    [StringLength(255)]
    public string? VerificationToken { get; set; }
}


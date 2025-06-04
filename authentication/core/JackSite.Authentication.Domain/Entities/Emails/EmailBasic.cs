using JackSite.Authentication.Enums.Emails;
using JackSite.Domain.Enums;

namespace JackSite.Authentication.Entities.Emails;

public class EmailBasic : DraftableEntity
{
    public string Title { get; set; } = null!;
    
    public SendEmailType Type { get; set; }
    
    /// <summary>
    /// 邮件唯一标识符（SMTP生成）
    /// </summary>
    public string? MessageId { get; set; }
    
    /// <summary>
    /// 发件人邮箱
    /// </summary>
    public string SenderEmail { get; set; } = null!;
    
    /// <summary>
    /// 发件人显示名称
    /// </summary>
    public string? SenderName { get; set; }
    
    /// <summary>
    /// 邮件发送状态
    /// </summary>
    public EmailStatus Status { get; set; } = EmailStatus.Draft;
    
    /// <summary>
    /// 邮件重要性
    /// </summary>
    public EmailPriorityType Importance { get; set; } = EmailPriorityType.Normal;
    
    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentTime { get; set; }
    
    /// <summary>
    /// 最后尝试发送时间
    /// </summary>
    public DateTime? LastTryTime { get; set; }
    
    /// <summary>
    /// 尝试发送次数
    /// </summary>
    public int RetryCount { get; set; } = 0;
    
    /// <summary>
    /// 发送失败原因
    /// </summary>
    public string? FailureReason { get; set; }
    
    public virtual ICollection<EmailRecipient> EmailRecipients { get; set; } = [];

    public virtual EmailContent? EmailContent { get; set; }
    
    public virtual ICollection<EmailAttachment> EmailAttachments { get; set; } = [];
}

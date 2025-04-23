

namespace JackSite.Notification.Server.Entities;

public class Email : EntityBase
{
    [Required]
    [MaxLength(256)]
    public string From { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    public string To { get; set; } = null!;

    [Required]
    [MaxLength(512)]
    public string Subject { get; set; } = null!;

    [Required]
    public EmailStatus Status { get; set; }

    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }

    public int RetryCount { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? SentAt { get; set; }

    public DateTime? LastRetryAt { get; set; }

    [MaxLength(100)]
    public string? TemplateName { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? TemplateData { get; set; } // JSON格式存储模板数据

    // 导航属性
    [Required]
    public EmailContent Content { get; set; } = null!;

    public ICollection<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
}

public enum EmailStatus
{
    Pending = 0,
    Sending = 1,
    Sent = 2,
    Failed = 3,
    Cancelled = 4
}
using JackSite.Authentication.Enums.Emails;

namespace JackSite.Authentication.Entities.Emails;

public class EmailBasic : DraftableEntity
{
    [Description("邮件标题")] public string Title { get; set; } = null!;

    [Description("邮件发送类型")] public SendEmailType Type { get; set; }

    [Description("邮件唯一标识符")] public string? MessageId { get; set; }

    [Description("发件人邮箱")] public string SenderEmail { get; set; } = null!;

    [Description("发件人显示名称")] public string? SenderName { get; set; }

    [Description("邮件发送状态")] public EmailStatus Status { get; set; } = EmailStatus.Draft;

    [Description("邮件重要性")] public EmailPriorityType Importance { get; set; } = EmailPriorityType.Normal;

    [Description("发送时间")] public DateTimeOffset? SentTime { get; set; }

    [Description("最后尝试发送时间")] public DateTimeOffset? LastTryTime { get; set; }

    [Description("尝试发送次数")] public int RetryCount { get; set; }

    [Description("发送失败原因")] public string? FailureReason { get; set; }

    [Description("邮件收件人列表")] public virtual ICollection<EmailRecipient> EmailRecipients { get; set; } = [];

    [Description("邮件内容")] public virtual EmailContent? EmailContent { get; set; }

    [Description("邮件附件列表")] public virtual ICollection<EmailAttachment> EmailAttachments { get; set; } = [];
}
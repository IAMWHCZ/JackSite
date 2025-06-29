using JackSite.Authentication.Enums.Emails;

namespace JackSite.Authentication.Entities.Emails;

public class EmailRecipient : DraftableEntity
{
    [Description("收件人邮箱")] public string RecipientEmail { get; set; } = null!;

    [Description("收件人名称")] public string? RecipientName { get; set; }

    [Description("收件人类型")] public RecipientType Type { get; set; } = RecipientType.To;

    [Description("发送状态")] public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;

    [Description("送达时间")] public DateTimeOffset? DeliveredTime { get; set; }

    [Description("阅读时间")] public DateTimeOffset? ReadTime { get; set; }

    [Description("跟踪ID")] public string? TrackingId { get; set; }

    [Description("送达失败原因")] public string? FailureReason { get; set; }

    [Description("邮件记录ID")]
    public Guid EmailRecordId { get; set; }

    [Description("关联的邮件基础信息")]
    public virtual EmailBasic EmailBasic { get; set; } = null!;
}
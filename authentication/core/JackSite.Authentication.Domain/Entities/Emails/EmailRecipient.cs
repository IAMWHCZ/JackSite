using JackSite.Authentication.Enums.Emails;

namespace JackSite.Authentication.Entities.Emails;

public class EmailRecipient : DraftableEntity
{
    /// <summary>
    /// 收件人邮箱
    /// </summary>
    public string RecipientEmail { get; set; } = null!;
    
    /// <summary>
    /// 收件人名称
    /// </summary>
    public string? RecipientName { get; set; }
    
    /// <summary>
    /// 收件人类型（主收件人、抄送、密送）
    /// </summary>
    public RecipientType Type { get; set; } = RecipientType.To;
    
    /// <summary>
    /// 发送状态
    /// </summary>
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;
    
    /// <summary>
    /// 送达时间
    /// </summary>
    public DateTime? DeliveredTime { get; set; }
    
    /// <summary>
    /// 阅读时间
    /// </summary>
    public DateTime? ReadTime { get; set; }
    
    /// <summary>
    /// 跟踪ID（用于跟踪邮件阅读和点击）
    /// </summary>
    public string? TrackingId { get; set; }
    
    /// <summary>
    /// 送达失败原因
    /// </summary>
    public string? FailureReason { get; set; }
    
    public long EmailRecordId { get; set; }
    
    public virtual EmailBasic EmailBasic { get; set; } = null!;
}




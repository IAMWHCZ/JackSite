namespace JackSite.Authentication.Enums.Emails;

/// <summary>
/// 送达状态枚举
/// </summary>
public enum DeliveryStatus
{
    Pending = 0,       // 待发送
    Sent = 1,          // 已发送
    Delivered = 2,     // 已送达
    Failed = 3,        // 发送失败
    Bounced = 4,       // 邮件退回
    Read = 5           // 已阅读
}

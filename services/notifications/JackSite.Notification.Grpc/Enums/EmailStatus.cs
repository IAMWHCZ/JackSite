namespace JackSite.Notification.Grpc.Enums;

/// <summary>
/// 邮件发送状态
/// </summary>
public enum EmailStatus
{
    /// <summary>
    /// 待发送
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// 发送中
    /// </summary>
    Sending = 1,
    
    /// <summary>
    /// 已发送
    /// </summary>
    Sent = 2,
    
    /// <summary>
    /// 发送失败
    /// </summary>
    Failed = 3
}
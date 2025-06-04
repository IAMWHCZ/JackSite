namespace JackSite.Authentication.Enums.Emails;

public enum EmailStatus
{
    Draft = 0,        // 草稿
    Queued = 1,       // 队列中
    Sending = 2,      // 发送中
    Sent = 3,         // 已发送
    Failed = 4,       // 发送失败
    Cancelled = 5     // 已取消
}
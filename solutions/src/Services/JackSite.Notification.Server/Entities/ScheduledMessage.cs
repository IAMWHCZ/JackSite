using JackSite.Common.Domain;
using JackSite.Notification.Server.Enums;

namespace JackSite.Notification.Server.Entities;

public class ScheduledMessage : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public SnowflakeId UserId { get; set; }
    public SnowflakeId[]? UserIds { get; set; }
    public DateTime ScheduledTime { get; set; }
    public DateTime? SentTime { get; set; }
    public bool IsSent { get; set; }
    public int RetryCount { get; set; }
    public string? ErrorMessage { get; set; }
    public MessageStatus Status { get; set; }
    public string? CronExpression { get; set; }  // 用于重复性的定时任务
    public bool IsRecurring { get; set; }
}

public enum MessageStatus
{
    Pending,
    Processing,
    Sent,
    Failed,
    Cancelled
}
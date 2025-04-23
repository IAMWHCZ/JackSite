using JackSite.Notification.Server.Enums;

namespace JackSite.Notification.Server.Contracts;

public interface INotificationService
{
    /// <summary>
    /// 向指定用户发送通知
    /// </summary>
    /// <param name="userId">接收通知的用户ID</param>
    /// <param name="title">通知标题</param>
    /// <param name="message">通知内容</param>
    /// <param name="type">通知类型（邮件、短信、系统通知等）</param>
    Task SendNotificationAsync(SnowflakeId userId, string title, string message, NotificationType type);

    /// <summary>
    /// 向多个用户发送相同的通知
    /// </summary>
    /// <param name="userIds">接收通知的用户ID集合</param>
    /// <param name="title">通知标题</param>
    /// <param name="message">通知内容</param>
    /// <param name="type">通知类型（邮件、短信、系统通知等）</param>
    Task SendNotificationToMultipleUsersAsync(IEnumerable<SnowflakeId> userIds, string title, string message, NotificationType type);

    /// <summary>
    /// 获取指定用户的通知列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="includeRead">是否包含已读通知，默认为false</param>
    /// <returns>通知列表</returns>
    Task<IEnumerable<Entities.Notification>> GetUserNotificationsAsync(SnowflakeId userId, bool includeRead = false);

    /// <summary>
    /// 将指定通知标记为已读
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="notificationId">通知ID</param>
    Task MarkAsReadAsync(SnowflakeId userId, SnowflakeId notificationId);

    /// <summary>
    /// 将用户的所有未读通知标记为已读
    /// </summary>
    /// <param name="userId">用户ID</param>
    Task MarkAllAsReadAsync(SnowflakeId userId);

    /// <summary>
    /// 删除指定的通知
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="notificationId">要删除的通知ID</param>
    Task DeleteNotificationAsync(SnowflakeId userId, SnowflakeId notificationId);

    /// <summary>
    /// 删除用户的所有通知
    /// </summary>
    /// <param name="userId">用户ID</param>
    Task DeleteAllNotificationsAsync(SnowflakeId userId);
}
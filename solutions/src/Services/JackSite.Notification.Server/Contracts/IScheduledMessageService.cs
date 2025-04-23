using JackSite.Notification.Server.Enums;

namespace JackSite.Notification.Server.Contracts;

public interface IScheduledMessageService
{
    /// <summary>
    /// 创建定时消息
    /// </summary>
    /// <param name="title">消息标题</param>
    /// <param name="message">消息内容</param>
    /// <param name="type">消息类型（邮件、短信、系统通知等）</param>
    /// <param name="scheduledTime">计划发送时间</param>
    /// <param name="userId">创建者用户ID</param>
    /// <param name="userIds">可选的额外接收者用户ID数组</param>
    /// <param name="cronExpression">可选的Cron表达式，用于设置重复发送规则</param>
    /// <remarks>
    /// 如果提供了cronExpression，消息将按照Cron表达式指定的时间规则重复发送。
    /// Cron表达式示例：
    /// - "0 0 * * *" 每天午夜执行
    /// - "0 */15 * * *" 每15分钟执行一次
    /// </remarks>
    Task CreateScheduledMessageAsync(
        string title,
        string message,
        NotificationType type,
        DateTime scheduledTime,
        SnowflakeId userId,
        SnowflakeId[]? userIds = null,
        string? cronExpression = null);
    
    /// <summary>
    /// 取消指定的定时消息
    /// </summary>
    /// <param name="messageId">要取消的消息ID</param>
    /// <remarks>
    /// 取消后的消息将不会被发送，如果是重复性消息，也不会继续创建新的实例
    /// </remarks>
    Task CancelScheduledMessageAsync(Guid messageId);

    /// <summary>
    /// 获取所有待处理的定时消息
    /// </summary>
    /// <returns>待处理的定时消息列表</returns>
    /// <remarks>
    /// 返回的消息包括：
    /// - 状态为Pending的消息
    /// - 计划发送时间小于等于当前时间的消息
    /// </remarks>
    Task<IEnumerable<ScheduledMessage>> GetPendingMessagesAsync();

    /// <summary>
    /// 处理指定的定时消息
    /// </summary>
    /// <param name="message">要处理的定时消息实体</param>
    /// <remarks>
    /// 处理过程包括：
    /// 1. 更新消息状态为Processing
    /// 2. 发送通知
    /// 3. 如果是重复性消息，创建下一次的定时消息
    /// 4. 更新消息状态为Sent或Failed
    /// </remarks>
    Task ProcessScheduledMessageAsync(ScheduledMessage message);
}

using JackSite.Notification.Server.Enums;

namespace JackSite.Notification.Server.Services;

[ServiceLifetime(ServiceLifetime.Scoped)]
public class NotificationService(
    NotificationDbContext dbContext,
    IHubContext<NotificationHub> hubContext,
    ILogger<NotificationService> logger)
    : INotificationService
{
    public async Task SendNotificationAsync(SnowflakeId userId, string title, string message, NotificationType type)
    {
        var notification = new Entities.Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            // 保存到数据库
            dbContext.Notification.Add(notification);
            await dbContext.SaveChangesAsync();

            // 实时推送到客户端
            await hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", notification);

            logger.LogInformation(
                "Notification sent to user {UserId}: {Title}",
                userId, title);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to send notification to user {UserId}",
                userId);
            throw;
        }
    }

    public async Task SendNotificationToMultipleUsersAsync(
        IEnumerable<SnowflakeId> userIds,
        string title,
        string message,
        NotificationType type)
    {
        var enumerable = userIds as SnowflakeId[] ?? userIds.ToArray();
        var notifications = enumerable.Select(userId => new Entities.Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        try
        {
            // 批量保存到数据库
            dbContext.Notification.AddRange(notifications);
            await dbContext.SaveChangesAsync();

            // 并行推送到所有用户
            var tasks = notifications.Select(notification =>
                hubContext.Clients.User(notification.UserId.ToString())
                    .SendAsync("ReceiveNotification", notification));

            await Task.WhenAll(tasks);

            logger.LogInformation(
                "Bulk notification sent to {Count} users: {Title}",
                enumerable.Length, title);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to send bulk notification to {Count} users",
                enumerable.Length);
            throw;
        }
    }

    public async Task<IEnumerable<Entities.Notification>> GetUserNotificationsAsync(
        SnowflakeId userId,
        bool includeRead = false)
    {
        return await dbContext.Notification
            .Where(n => n.UserId == userId)
            .Where(n => !n.IsRead || includeRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(SnowflakeId userId, SnowflakeId notificationId)
    {
        var notification = await dbContext.Notification
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        if (notification == null) return;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(SnowflakeId userId)
    {
        var notifications = await dbContext.Notification
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        var now = DateTime.UtcNow;
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadAt = now;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteNotificationAsync(SnowflakeId userId, SnowflakeId notificationId)
    {
        var notification = await dbContext.Notification
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        if (notification == null) return;

        dbContext.Notification.Remove(notification);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllNotificationsAsync(SnowflakeId userId)
    {
        var notifications = await dbContext.Notification
            .Where(n => n.UserId == userId)
            .ToListAsync();

        dbContext.Notification.RemoveRange(notifications);
        await dbContext.SaveChangesAsync();
    }
}
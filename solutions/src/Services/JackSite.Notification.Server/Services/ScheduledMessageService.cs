using Cronos;
using JackSite.Notification.Server.Enums;

namespace JackSite.Notification.Server.Services;

[ServiceLifetime(ServiceLifetime.Scoped)]
public class ScheduledMessageService(
    NotificationDbContext dbContext,
    INotificationService notificationService,
    ILogger<ScheduledMessageService> logger):IScheduledMessageService
{
    public async Task CreateScheduledMessageAsync(
        string title,
        string message,
        NotificationType type,
        DateTime scheduledTime,
        SnowflakeId userId,
        SnowflakeId[]? userIds = null,
        string? cronExpression = null)
    {
        var scheduledMessage = new ScheduledMessage
        {
            Title = title,
            Message = message,
            Type = type,
            ScheduledTime = scheduledTime,
            UserId = userId,
            UserIds = userIds,
            Status = MessageStatus.Pending,
            CronExpression = cronExpression,
            IsRecurring = !string.IsNullOrEmpty(cronExpression)
        };

        dbContext.ScheduledMessages.Add(scheduledMessage);
        await dbContext.SaveChangesAsync();
    }

    public async Task CancelScheduledMessageAsync(Guid messageId)
    {
        var message = await dbContext.ScheduledMessages.FindAsync(messageId);
        if (message == null) return;

        message.Status = MessageStatus.Cancelled;
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ScheduledMessage>> GetPendingMessagesAsync()
    {
        return await dbContext.ScheduledMessages
            .Where(m => m.Status == MessageStatus.Pending)
            .Where(m => m.ScheduledTime <= DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task ProcessScheduledMessageAsync(ScheduledMessage message)
    {
        try
        {
            message.Status = MessageStatus.Processing;
            await dbContext.SaveChangesAsync();

            await notificationService.SendNotificationAsync(
                message.UserId,
                message.Title,
                message.Message,
                message.Type);

            message.Status = MessageStatus.Sent;
            message.SentTime = DateTime.UtcNow;

            if (message.IsRecurring)
            {
                await ScheduleNextOccurrence(message);
            }
        }
        catch (Exception ex)
        {
            message.Status = MessageStatus.Failed;
            message.ErrorMessage = ex.Message;
            message.RetryCount++;
            logger.LogError(ex, "Failed to process scheduled message {MessageId}", message.Id);
        }
        finally
        {
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task ScheduleNextOccurrence(ScheduledMessage message)
    {
        if (string.IsNullOrEmpty(message.CronExpression)) return;

        try
        {
            var cronExpression = CronExpression.Parse(message.CronExpression);
            var nextOccurrence = cronExpression.GetNextOccurrence(DateTime.UtcNow);

            if (nextOccurrence.HasValue)
            {
                await CreateScheduledMessageAsync(
                    message.Title,
                    message.Message,
                    message.Type,
                    nextOccurrence.Value,
                    message.UserId,
                    message.UserIds,
                    message.CronExpression);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to schedule next occurrence for message {MessageId}", message.Id);
        }
    }
}
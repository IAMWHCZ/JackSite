using JackSite.Notification.Server.Enums;

namespace JackSite.Notification.Server.Handlers.Command;

public record SendNotificationCommand(SnowflakeId UserId, string Title, string Message, NotificationType Type) : ICommand<Unit>;

internal sealed class SendNotificationHandler(NotificationDbContext dbContext)
    : IRequestHandler<SendNotificationCommand, Unit>
{
    public Task<Unit> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        dbContext.Notification.Add(new Entities.Notification
        {
            Title = request.Title,
            Message = request.Message,
            Type = request.Type,
            UserId = request.UserId
        });
        return Unit.Task;
    }
}
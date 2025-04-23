using JackSite.Common.Domain;
using JackSite.Notification.Server.Enums;
using Microsoft.AspNetCore.SignalR;

namespace JackSite.Notification.Server.Entities;

public class Notification : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public SnowflakeId UserId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
}

public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string title, string message, NotificationType type)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", new Notification
        {
            Title = title,
            Message = message,
            Type = type,
            UserId = SnowflakeId.Parse(userId) 
        });
    }
}

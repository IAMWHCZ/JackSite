namespace JackSite.Notification.Server.Configs;

public class ScheduledMessageConfig
{
    public bool Enabled { get; set; } = true;
    public int IntervalMinutes { get; set; } = 1;
    public int BatchSize { get; set; } = 100;
    public int MaxRetries { get; set; } = 3;
    public int RetryDelayMinutes { get; set; } = 5;
}
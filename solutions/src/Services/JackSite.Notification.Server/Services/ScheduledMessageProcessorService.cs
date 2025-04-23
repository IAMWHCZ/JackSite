using Microsoft.Extensions.Options;
using JackSite.Notification.Server.Configs;

namespace JackSite.Notification.Server.Services;

public class ScheduledMessageProcessorService(
    IServiceProvider serviceProvider,
    ILogger<ScheduledMessageProcessorService> logger,
    IOptions<ScheduledMessageConfig> config)
    : BackgroundService
{
    private readonly ScheduledMessageConfig _config = config.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_config.Enabled)
        {
            logger.LogInformation("ScheduledMessageProcessor is disabled");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessages(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing scheduled messages");
            }

            await Task.Delay(TimeSpan.FromMinutes(_config.IntervalMinutes), stoppingToken);
        }
    }

    private async Task ProcessPendingMessages(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<ScheduledMessageService>();

        var pendingMessages = await messageService.GetPendingMessagesAsync();
        var messagesToProcess = pendingMessages.Take(_config.BatchSize);

        foreach (var message in messagesToProcess)
        {
            if (stoppingToken.IsCancellationRequested) break;

            if (message.RetryCount >= _config.MaxRetries)
            {
                logger.LogWarning(
                    "Message {MessageId} exceeded maximum retry attempts",
                    message.Id);
                continue;
            }

            try
            {
                await messageService.ProcessScheduledMessageAsync(message);
                logger.LogInformation(
                    "Successfully processed scheduled message {MessageId}",
                    message.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to process scheduled message {MessageId}",
                    message.Id);
            }
        }
    }
}
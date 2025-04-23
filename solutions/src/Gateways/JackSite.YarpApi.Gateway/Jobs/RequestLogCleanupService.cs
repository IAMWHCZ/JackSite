using JackSite.YarpApi.Gateway.Data;
using Microsoft.EntityFrameworkCore;

namespace JackSite.YarpApi.Gateway.Jobs;

public class RequestLogCleanupService(
    IServiceProvider serviceProvider,
    ILogger<RequestLogCleanupService> logger,
    IConfiguration configuration)
    : BackgroundService
{
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(
        configuration.GetValue("RequestLogCleanup:IntervalHours", 24));
    private readonly TimeSpan _retentionPeriod = TimeSpan.FromDays(
        configuration.GetValue("RequestLogCleanup:RetentionDays", 7));

    // 从配置中读取清理间隔和保留期限，如果没有配置则使用默认值

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupOldRequestLogs(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while cleaning up old request logs");
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }

    private async Task CleanupOldRequestLogs(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GatewayDbContext>();

        var cutoffDate = DateTime.UtcNow.Subtract(_retentionPeriod);

        logger.LogInformation("Starting cleanup of request logs older than {CutoffDate}", cutoffDate);

        try
        {
            // 使用批量删除以提高性能
            var deletedCount = await dbContext.RequestLogs
                .Where(r => r.RequestTime < cutoffDate)
                .Where(x=>x.StatusCode == 200)
                .ExecuteDeleteAsync(stoppingToken);

            logger.LogInformation("Successfully deleted {DeletedCount} old request logs", deletedCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to cleanup old request logs");
            throw;
        }
    }
}
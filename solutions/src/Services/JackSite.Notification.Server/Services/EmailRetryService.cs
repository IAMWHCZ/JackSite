using JackSite.Notification.Server.Contracts;
using JackSite.Notification.Server.Entities;

public class EmailRetryService(
    ILogger<EmailRetryService> logger,
    IConfiguration configuration,
    IEmailService emailService,
    NotificationDbContext dbContext)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessFailedEmails(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing failed emails");
            }

            // 等待配置的间隔时间
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task ProcessFailedEmails(CancellationToken stoppingToken)
    {
        {
            var maxRetries = configuration.GetValue("Email:MaxRetries", 3);
            var retryWindow = TimeSpan.FromHours(configuration.GetValue<int>("Email:RetryWindowHours", 24));

            var failedEmails = await dbContext.Emails
                .Include(e => e.Content)
                .Include(e => e.Attachments)
                .Where(e => e.Status == EmailStatus.Failed)
                .Where(e => e.RetryCount < maxRetries)
                .Where(e => e.CreatedAt > DateTime.UtcNow.Subtract(retryWindow))
                .ToListAsync(stoppingToken);

            foreach (var email in failedEmails)
            {
                try
                {
                    var attachmentPaths = email.Attachments.Select(a => a.FilePath);
                    await emailService.SendEmailAsync(email.To, email.Subject, email.Content.Body, attachmentPaths);

                    logger.LogInformation("Successfully retried sending email to {To}", email.To);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to retry sending email to {To}", email.To);
                }
            }
        }
    }
}
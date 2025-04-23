using System.Collections.Concurrent;
using JackSite.Common.DependencyInjection;
using JackSite.Notification.Server.Configs;
using JackSite.Notification.Server.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Scriban;

namespace JackSite.Notification.Server.Services;

[ServiceLifetime(ServiceLifetime.Scoped)]
public class EmailService(IOptions<EmailConfig> emailConfig, ILogger<EmailService> logger,NotificationDbContext dbContext)
    : IEmailService
{
    private readonly EmailConfig _emailConfig = emailConfig.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        await SendEmailAsync(to, subject, body, Enumerable.Empty<string>());
    }

    public async Task SendEmailAsync(string to, string subject, string body, string attachmentPath)
    {
        await SendEmailAsync(to, subject, body, new[] { attachmentPath });
    }

    public async Task SendEmailAsync(string to, string subject, string body, IEnumerable<string> attachmentPaths)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };

            foreach (var attachmentPath in attachmentPaths.Where(File.Exists))
            {
                builder.Attachments.Add(attachmentPath);
            }

            email.Body = builder.ToMessageBody();

            await SendEmailMessageAsync(email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {To}", to);
            throw;
        }
    }

    public async Task SendEmailAsync(IEnumerable<string> to, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
            
            foreach (var recipient in to)
            {
                email.To.Add(MailboxAddress.Parse(recipient));
            }
            
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            await SendEmailMessageAsync(email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send bulk email");
            throw;
        }
    }

    public async Task SendEmailWithTemplateAsync(string to, string subject, string templateName, object model)
    {
        try
        {
            var templatePath = Path.Combine("EmailTemplates", $"{templateName}.html");
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Email template {templateName} not found");
            }

            var templateContent = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(templateContent);
            var body = await template.RenderAsync(model);

            await SendEmailAsync(to, subject, body);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send template email to {To}", to);
            throw;
        }
    }

    public async Task SendBulkEmailAsync(IEnumerable<(string Email, string Subject, string Body)> emails)
    {
        try
        {
            using var smtp = new SmtpClient();
            await ConnectSmtpAsync(smtp);

            foreach (var (email, subject, body) in emails)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                try
                {
                    await smtp.SendAsync(message);
                    logger.LogInformation("Email sent successfully to {To}", email);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send email to {To}", email);
                    // 继续发送其他邮件
                }
            }

            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize SMTP client for bulk email");
            throw;
        }
    }

    public async Task ProcessEmailBatchAsync(
        List<(string Email, string Subject, string Body, IEnumerable<string>? Attachments)> batch,
        SemaphoreSlim semaphore,
        ConcurrentBag<(string Email, Exception Exception)> failedEmails)
    {
        var tasks = new List<Task>();

        foreach (var (email, subject, body, attachments) in batch)
        {
            tasks.Add(ProcessSingleEmailAsync(email, subject, body, attachments, semaphore, failedEmails));
        }

        await Task.WhenAll(tasks);
    }

    private async Task ProcessSingleEmailAsync(
        string email,
        string subject,
        string body,
        IEnumerable<string>? attachments,
        SemaphoreSlim semaphore,
        ConcurrentBag<(string Email, Exception Exception)> failedEmails)
    {
        try
        {
            await semaphore.WaitAsync();
            
            try
            {
                await SendEmailAsync(email, subject, body, attachments ?? Enumerable.Empty<string>());
                logger.LogInformation("Successfully sent email to {Email}", email);
            }
            catch (Exception ex)
            {
                failedEmails.Add((email, ex));
                logger.LogError(ex, "Failed to send email to {Email}", email);
            }
            finally
            {
                semaphore.Release();
            }
        }
        catch (Exception ex)
        {
            failedEmails.Add((email, ex));
            logger.LogError(ex, "Failed to process email to {Email}", email);
        }
    }

    public async Task DeleteEmailAsync(SnowflakeId id)
    {
        var emails = await dbContext.Emails.AsNoTracking().Where(x=>x.Id == id).ToListAsync();
        dbContext.Emails.RemoveRange(emails);
        await dbContext.SaveChangesAsync();
    }

    private async Task SendEmailMessageAsync(MimeMessage message)
    {
        using var smtp = new SmtpClient();
        await ConnectSmtpAsync(smtp);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
        
        logger.LogInformation("Email sent successfully to {To}", string.Join(", ", message.To.Select(x => x.ToString())));
    }

    private async Task ConnectSmtpAsync(SmtpClient smtp)
    {
        await smtp.ConnectAsync(_emailConfig.Host, _emailConfig.Port,
            _emailConfig.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
        await smtp.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
    }
}

using System.Net;
using System.Net.Mail;
using JackSite.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace JackSite.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IBaseRepository<EmailRecord, Guid> _emailRepository;
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpClient _smtpClient;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(
        IBaseRepository<EmailRecord, Guid> emailRepository,
        IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _emailRepository = emailRepository;
        _logger = logger;

        // 从配置中读取邮件设置
        var emailConfig = configuration.GetSection("Email");
        _senderEmail = emailConfig["SenderEmail"] ?? throw new InvalidOperationException("SenderEmail not configured");
        _senderName = emailConfig["SenderName"] ?? "JackSite System";

        // 配置SMTP客户端
        _smtpClient = new SmtpClient
        {
            Host = emailConfig["SmtpServer"] ?? "smtp-mail.outlook.com",
            Port = int.Parse(emailConfig["SmtpPort"] ?? "587"),
            EnableSsl = bool.Parse(emailConfig["EnableSsl"] ?? "true"),
            Timeout = int.Parse(emailConfig["Timeout"] ?? "30000"),
            UseDefaultCredentials = bool.Parse(emailConfig["UseDefaultCredentials"] ?? "false"),
            Credentials = new NetworkCredential(_senderEmail, emailConfig["Password"])
        };
    }

    public async Task SendEmailAsync(string to, string message, SendEmailType type)
    {
        await SendEmailWithSubjectAsync(to, "Message from JackSite", message, type);
    }

    public async Task SendEmailWithSubjectAsync(string to, string subject, string message, SendEmailType type)
    {
        var emailRecord = new EmailRecord
        {
            Receiver = to,
            Sender = _senderEmail,
            Subject = subject,
            Message = message,
            Status = EmailStatus.Pending,
            IsHtml = false,
            SendEmailType = type
        };

        await _emailRepository.AddAsync(emailRecord);
        await SendEmail(emailRecord);
    }

    public async Task SendBulkEmailsAsync(List<string> toList, string message, SendEmailType type)
    {
        foreach (var to in toList)
        {
            await SendEmailAsync(to, message, type);
        }
    }

    public async Task SendHtmlEmailAsync(string to, string htmlContent, SendEmailType type)
    {
        var emailRecord = new EmailRecord
        {
            Receiver = to,
            Sender = _senderEmail,
            Subject = "HTML Message from JackSite",
            Message = htmlContent,
            Status = EmailStatus.Pending,
            IsHtml = true,
            SendEmailType = type
        };

        await _emailRepository.AddAsync(emailRecord);
        await SendEmail(emailRecord);
    }

    public async Task<string> GetEmailStatusAsync(Guid emailId)
    {
        var emailRecord = await _emailRepository.GetByIdAsync(emailId);
        return emailRecord?.Status.ToString() ?? "Not Found";
    }

    public async Task SetEmailPriorityAsync(string to, string message, EmailPriorityType priority)
    {
        var emailRecord = new EmailRecord
        {
            Receiver = to,
            Sender = _senderEmail,
            Message = message,
            Subject = "Priority Email",
            SentDate = DateTime.UtcNow,
            Status = EmailStatus.Pending,
            Priority = priority
        };

        await _emailRepository.AddAsync(emailRecord);
        await SendEmail(emailRecord);
    }

    private async Task SendEmail(EmailRecord emailRecord)
    {
        try
        {
            _logger.LogInformation("正在发送邮件到 {Receiver}, 主题: {Subject}", 
                emailRecord.Receiver, emailRecord.Subject);

            // 更新状态为发送中
            emailRecord.Status = EmailStatus.Sending;
            await _emailRepository.UpdateAsync(emailRecord);

            // 创建邮件消息
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail, _senderName),
                Subject = emailRecord.Subject,
                Body = emailRecord.Message,
                IsBodyHtml = emailRecord.IsHtml,
                Priority = ConvertPriority(emailRecord.Priority)
            };

            mailMessage.To.Add(emailRecord.Receiver);

            // 发送邮件
            await _smtpClient.SendMailAsync(mailMessage);

            // 更新状态为已完成
            emailRecord.Status = EmailStatus.Finished;
            emailRecord.SentDate = DateTime.UtcNow;
            await _emailRepository.UpdateAsync(emailRecord);

            _logger.LogInformation("邮件发送成功: {EmailId}", emailRecord.Id);
        }
        catch (Exception ex)
        {
            // 更新状态为失败
            emailRecord.Status = EmailStatus.Failed;
            emailRecord.ErrorMessage = ex.Message;
            await _emailRepository.UpdateAsync(emailRecord);

            _logger.LogError(ex, "邮件发送失败: {EmailId}, 错误: {ErrorMessage}", 
                emailRecord.Id, ex.Message);
            
            throw;
        }
    }

    private static MailPriority ConvertPriority(EmailPriorityType priority)
    {
        return priority switch
        {
            EmailPriorityType.High => MailPriority.High,
            EmailPriorityType.Low => MailPriority.Low,
            _ => MailPriority.Normal
        };
    }
}
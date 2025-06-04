using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using JackSite.Authentication.Entities.Emails;
using JackSite.Authentication.Exceptions;
using JackSite.Authentication.Interfaces.Services;
using JackSite.Authentication.ValueObjects;
using JackSite.Domain.Enums;

namespace JackSite.Authentication.Infrastructure.Services;

public class EmailService(
    ICacheService cacheService,
    IRepository<EmailBasic> emailRepository
) : IEmailService
{
    public async Task SendEmailAsync(
        string to,
        string subject,
        string body,
        SendEmailType type,
        bool isBodyHtml = true
    )
    {
        try
        {
            var email = new EmailBasic
            {
                Title = subject,
                Type = type,
            };
            email.EmailContent = new EmailContent
            {
                Content = body,
                Subject = subject,
                EmailId = email.Id
            };

            await emailRepository.AddAsync(email);
            await emailRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new EmailException(e.Message, to, e);
        }
    }

    public Task SendEmailAsync(
        string to,
        string subject,
        string body,
        SendEmailType type,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null,
        IEnumerable<string>? attachments = null,
        bool isBodyHtml = true
    )
    {
        throw new NotImplementedException();
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            // 尝试获取配置信息，验证配置是否有效
            var config = await GetConfigInfoAsync();
            if (config == null)
            {
                throw new EmailException("无法获取邮件配置信息");
            }

            using var smtpClientAsync = await GetSmtpClientAsync();
            using var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(config.FromEmail);

            mailMessage.To.Add(config.FromEmail);
            mailMessage.Subject = "凭证验证";
            mailMessage.Body = "这是一条测试SMTP凭证的消息";
            mailMessage.IsBodyHtml = false;
            await smtpClientAsync.SendMailAsync(mailMessage);
            
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(config.SmtpServer, config.SmtpPort);
            if (!tcpClient.Connected)
            {
                throw new EmailException($"无法连接到SMTP服务器 {config.SmtpServer}:{config.SmtpPort}");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            throw new EmailException("邮件服务连接测试失败", ex);
        }
    }

    public Task<string?> GetSendStatusAsync(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<EmailConfigInfo> GetConfigInfoAsync()
    {
        throw new NotImplementedException();
    }

    public Task SendBulkEmailAsync(IEnumerable<string> toList, string subject, string body, SendEmailType type,
        bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task<string> PreviewEmailAsync(string subject, string body, bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task SaveDraftAsync(string to, string subject, string body, SendEmailType type,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null, IEnumerable<string>? attachments = null, bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyEmailAsync(string email, string code, SendEmailType type)
    {
        throw new NotImplementedException();
    }
    
    private async Task<SmtpClient> GetSmtpClientAsync()
    {
        var config = await GetConfigInfoAsync();
        if (config == null)
        {
            throw new EmailException("无法获取邮件配置信息");
        }

        var smtpClient = new SmtpClient(config.SmtpServer, config.SmtpPort);
        smtpClient.Credentials = new NetworkCredential(config.UserName, config.Password);
        smtpClient.EnableSsl = config.EnableSsl;
        smtpClient.Timeout = 20000;
        
        return smtpClient;
    }
}
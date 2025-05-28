using JackSite.Authentication.Interfaces.Services;
using JackSite.Authentication.ValueObjects;
using JackSite.Domain.Enums;

namespace JackSite.Authentication.Infrastructure.Services;

public class EmailService(ICacheService cacheService): IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, SendEmailType type, bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailAsync(string to, string subject, string body, SendEmailType type, IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null, IEnumerable<string>? attachments = null, bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TestConnectionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetSendStatusAsync(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<EmailConfigInfo> GetConfigInfoAsync()
    {
        throw new NotImplementedException();
    }

    public Task SendBulkEmailAsync(IEnumerable<string> toList, string subject, string body, SendEmailType type, bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task<string> PreviewEmailAsync(string subject, string body, bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task SaveDraftAsync(string to, string subject, string body, SendEmailType type, IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null, IEnumerable<string>? attachments = null, bool isBodyHtml = true)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyEmailAsync(string email, string code, SendEmailType type)
    {
        throw new NotImplementedException();
    }
}
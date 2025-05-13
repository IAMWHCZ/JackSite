using JackSite.Domain.Enums;
using JackSite.Domain.Services;

namespace JackSite.Infrastructure.Services;

public class EmailService(IBaseRepository<EmailRecord> emailRepository) : IEmailService
{
    public Task SendEmailAsync(string to, string message)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailWithSubjectAsync(string to, string subject, string message)
    {
        throw new NotImplementedException();
    }

    public Task SendBulkEmailsAsync(List<string> toList, string message)
    {
        throw new NotImplementedException();
    }

    public Task SendHtmlEmailAsync(string to, string htmlContent)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetEmailStatusAsync(string emailId)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailPriorityAsync(string to, string message, EmailPriorityType priority)
    {
        throw new NotImplementedException();
    }
}
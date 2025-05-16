using System.Net.Mail;
using JackSite.Domain.Enums;
using JackSite.Domain.Services;

namespace JackSite.Infrastructure.Services;

public class EmailService(IBaseRepository<EmailRecord,Guid> emailRepository) : IEmailService
{

    public async Task SendEmailAsync(string to, string message)
    {
        var emailRecord = new EmailRecord
        {
            Receiver = to,
            Message = message,
            Status = EmailStatus.Pending
        };

        await emailRepository.AddAsync(emailRecord);
        await SendEmail(emailRecord);
    }

    public async Task SendEmailWithSubjectAsync(string to, string subject, string message)
    {
        var emailRecord = new EmailRecord
        {
            Receiver = to,
            Message = message,
            Subject = subject,
            SentDate = DateTime.UtcNow,
            Status = EmailStatus.Pending
        };

        await emailRepository.AddAsync(emailRecord);
        await SendEmail(emailRecord);
    }

    public async Task SendBulkEmailsAsync(List<string> toList, string message)
    {
        foreach (var to in toList)
        {
            var emailRecord = new EmailRecord
            {
                Receiver = to,
                Message = message,
                Subject = "Bulk Email",
                SentDate = DateTime.UtcNow,
                Status = EmailStatus.Pending
            };

            await emailRepository.AddAsync(emailRecord);
            await SendEmail(emailRecord);
        }
    }

    public async Task SendHtmlEmailAsync(string to, string htmlContent)
    {
        var emailRecord = new EmailRecord
        {
            Receiver = to,
            Message = htmlContent,
            Subject = "HTML Email",
            SentDate = DateTime.UtcNow,
            Status = EmailStatus.Pending,
            IsHtml = true
        };

        await emailRepository.AddAsync(emailRecord);
        await SendEmail(emailRecord);
    }

    public async Task<bool> ValidateEmailAsync(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GetEmailStatusAsync(Guid emailId)
    {
        var emailRecord = await emailRepository.GetByIdAsync(emailId);
        return emailRecord?.Status.ToString() ?? "Not Found";
    }

    public async Task SetEmailPriorityAsync(string to, string message, EmailPriorityType priority)
    {
        var emailRecord = new EmailRecord
        {
            Receiver = to,
            Message = message,
            Subject = "Priority Email",
            SentDate = DateTime.UtcNow,
            Status = EmailStatus.Pending,
            Priority = priority
        };

        await emailRepository.AddAsync(emailRecord);
        await SendEmail(emailRecord);
    }

    private async Task SendEmail(EmailRecord emailRecord)
    {
        // Here you would implement the actual email sending logic
        // For example, using SMTP or a third-party email service

        // Update the status to Sent after sending
        emailRecord.Status = EmailStatus.Sending;
        await emailRepository.UpdateAsync(emailRecord);
    }
}
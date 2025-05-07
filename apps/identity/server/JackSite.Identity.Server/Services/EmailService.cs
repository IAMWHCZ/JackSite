using System.Net;
using System.Net.Mail;
using JackSite.Identity.Server.Interfaces;

namespace JackSite.Identity.Server.Services
{
    public class EmailService(
        IConfiguration configuration,
        ILogger<EmailService> logger) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("Email:Smtp");
                
                var smtpClient = new SmtpClient
                {
                    Host = smtpSettings["Host"],
                    Port = int.Parse(smtpSettings["Port"]),
                    EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
                };
                
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["From"], smtpSettings["FromName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                
                mailMessage.To.Add(to);
                
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {EmailAddress}", to);
                return false;
            }
        }
    }
}
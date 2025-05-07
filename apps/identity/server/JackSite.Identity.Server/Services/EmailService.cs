using System.Net;
using System.Net.Mail;
using System.Text;

namespace JackSite.Identity.Server.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    }
    
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        
        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
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
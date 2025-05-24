namespace JackSite.Infrastructure.Options;

public class EmailOptions
{
    public string SmtpServer { get; set; } = "smtp-mail.outlook.com";
    public int SmtpPort { get; set; } = 587;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
    public int Timeout { get; set; } = 30000; // 30秒
    public bool UseDefaultCredentials { get; set; } = false;
}
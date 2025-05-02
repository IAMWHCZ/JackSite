using JackSite.Notification.Grpc.Data;
using JackSite.Notification.Grpc.Entities;
using JackSite.Notification.Grpc.Enums;
using JackSite.Notification.Grpc.Interfaces;
using JackSite.Notification.Grpc.Options;
using JackSite.Shared.Core.IdGenerator;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace JackSite.Notification.Grpc.Services;

/// <summary>
/// 邮件服务实现
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailOptions _emailOptions;
    private readonly NotificationDbContext _dbContext;
    
    public EmailService(
        ILogger<EmailService> logger, 
        EmailOptions emailOptions,
        NotificationDbContext dbContext)
    {
        _logger = logger;
        _emailOptions = emailOptions;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// 发送注册验证邮件
    /// </summary>
    public async Task<EmailLog> SendRegistrationVerificationEmailAsync(string email, string username, string verificationToken)
    {
        _logger.LogInformation("准备发送验证邮件到 {Email}", email);
        
        var subject = "请验证您的账户";
        var body = GenerateVerificationEmailBody(username, verificationToken);
        
        return await SendAndLogEmailAsync(email, subject, body, "RegistrationVerification", username, null, verificationToken);
    }
    
    /// <summary>
    /// 发送密码重置邮件
    /// </summary>
    public async Task<EmailLog> SendPasswordResetEmailAsync(string email, string username, string resetToken)
    {
        _logger.LogInformation("准备发送密码重置邮件到 {Email}", email);
        
        var subject = "重置您的密码";
        var body = GeneratePasswordResetEmailBody(username, resetToken);
        
        return await SendAndLogEmailAsync(email, subject, body, "PasswordReset", username, null, resetToken);
    }
    
    /// <summary>
    /// 发送欢迎邮件
    /// </summary>
    public async Task<EmailLog> SendWelcomeEmailAsync(string email, string username)
    {
        _logger.LogInformation("准备发送欢迎邮件到 {Email}", email);
        
        var subject = "欢迎加入 JackSite";
        var body = GenerateWelcomeEmailBody(username);
        
        return await SendAndLogEmailAsync(email, subject, body, "Welcome", username);
    }
    
    /// <summary>
    /// 发送通知邮件
    /// </summary>
    public async Task<EmailLog> SendNotificationEmailAsync(string email, string username, string subject, string message)
    {
        _logger.LogInformation("准备发送通知邮件到 {Email}", email);
        
        var body = GenerateNotificationEmailBody(username, message);
        
        return await SendAndLogEmailAsync(email, subject, body, "Notification", username);
    }
    
    /// <summary>
    /// 获取邮件状态
    /// </summary>
    public async Task<EmailLog?> GetEmailStatusAsync(Guid emailId)
    {
        return await _dbContext.EmailLogs.FindAsync(emailId);
    }
    
    /// <summary>
    /// 发送并记录邮件
    /// </summary>
    private async Task<EmailLog> SendAndLogEmailAsync(
        string email, 
        string subject, 
        string body, 
        string emailType, 
        string? username = null, 
        string? userId = null, 
        string? token = null)
    {
        // 创建邮件日志记录
        var emailLog = new EmailLog
        {
            Id = SnowflakeId.NewId(),
            ToEmail = email,
            Subject = subject,
            EmailType = emailType,
            Status = EmailStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Username = username,
            UserId = userId,
            VerificationToken = token
        };
        
        // 保存初始记录
        await _dbContext.EmailLogs.AddAsync(emailLog);
        await _dbContext.SaveChangesAsync();
        
        try
        {
            // 更新状态为发送中
            emailLog.Status = EmailStatus.Sending;
            _dbContext.EmailLogs.Update(emailLog);
            await _dbContext.SaveChangesAsync();
            
            // 发送邮件
            await SendEmailAsync(email, subject, body);
            
            // 更新状态为已发送
            emailLog.Status = EmailStatus.Sent;
            emailLog.LastModifiedAt = DateTime.UtcNow;
            _dbContext.EmailLogs.Update(emailLog);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("邮件已成功发送到 {Email}", email);
        }
        catch (Exception ex)
        {
            // 更新状态为发送失败
            emailLog.Status = EmailStatus.Failed;
            emailLog.ErrorMessage = ex.Message;
            _dbContext.EmailLogs.Update(emailLog);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogError(ex, "发送邮件失败: {Message}", ex.Message);
            throw;
        }
        
        return emailLog;
    }
    
    /// <summary>
    /// 发送邮件
    /// </summary>
    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        
        message.Body = bodyBuilder.ToMessageBody();
        
        try
        {
            using var client = new SmtpClient();
            
            // 连接到SMTP服务器
            await client.ConnectAsync(
                _emailOptions.SmtpServer, 
                _emailOptions.SmtpPort, 
                _emailOptions.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            
            // 如果需要身份验证
            if (!string.IsNullOrEmpty(_emailOptions.Username) && !string.IsNullOrEmpty(_emailOptions.Password))
            {
                await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);
            }
            
            // 发送邮件
            await client.SendAsync(message);
            
            // 断开连接
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送邮件失败: {Message}", ex.Message);
            throw;
        }
    }
    
    /// <summary>
    /// 生成验证邮件内容
    /// </summary>
    private string GenerateVerificationEmailBody(string username, string verificationToken)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>验证您的账户</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 10px; text-align: center; }}
        .content {{ padding: 20px; border: 1px solid #ddd; }}
        .button {{ display: inline-block; background-color: #4CAF50; color: white; padding: 10px 20px; 
                  text-decoration: none; border-radius: 4px; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>欢迎加入 JackSite</h2>
        </div>
        <div class='content'>
            <p>您好，{username}！</p>
            <p>感谢您注册 JackSite。请点击下面的按钮验证您的电子邮件地址：</p>
            <p style='text-align: center;'>
                <a href='https://jacksite.com/verify?token={verificationToken}' class='button'>验证我的账户</a>
            </p>
            <p>或者，您可以复制以下链接并粘贴到浏览器地址栏中：</p>
            <p>https://jacksite.com/verify?token={verificationToken}</p>
            <p>如果您没有注册 JackSite 账户，请忽略此邮件。</p>
            <p>谢谢！<br>JackSite 团队</p>
        </div>
        <div class='footer'>
            <p>此邮件由系统自动发送，请勿回复。</p>
        </div>
    </div>
</body>
</html>";
    }
    
    /// <summary>
    /// 生成密码重置邮件内容
    /// </summary>
    private string GeneratePasswordResetEmailBody(string username, string resetToken)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>重置您的密码</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2196F3; color: white; padding: 10px; text-align: center; }}
        .content {{ padding: 20px; border: 1px solid #ddd; }}
        .button {{ display: inline-block; background-color: #2196F3; color: white; padding: 10px 20px; 
                  text-decoration: none; border-radius: 4px; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>重置您的密码</h2>
        </div>
        <div class='content'>
            <p>您好，{username}！</p>
            <p>我们收到了重置您 JackSite 账户密码的请求。请点击下面的按钮重置您的密码：</p>
            <p style='text-align: center;'>
                <a href='https://jacksite.com/reset-password?token={resetToken}' class='button'>重置我的密码</a>
            </p>
            <p>或者，您可以复制以下链接并粘贴到浏览器地址栏中：</p>
            <p>https://jacksite.com/reset-password?token={resetToken}</p>
            <p>如果您没有请求重置密码，请忽略此邮件，您的账户将保持安全。</p>
            <p>谢谢！<br>JackSite 团队</p>
        </div>
        <div class='footer'>
            <p>此邮件由系统自动发送，请勿回复。</p>
        </div>
    </div>
</body>
</html>";
    }
    
    /// <summary>
    /// 生成欢迎邮件内容
    /// </summary>
    private string GenerateWelcomeEmailBody(string username)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>欢迎加入 JackSite</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #673AB7; color: white; padding: 10px; text-align: center; }}
        .content {{ padding: 20px; border: 1px solid #ddd; }}
        .button {{ display: inline-block; background-color: #673AB7; color: white; padding: 10px 20px; 
                  text-decoration: none; border-radius: 4px; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>欢迎加入 JackSite</h2>
        </div>
        <div class='content'>
            <p>您好，{username}！</p>
            <p>欢迎加入 JackSite 社区！我们很高兴您成为我们的一员。</p>
            <p>在 JackSite，您可以：</p>
            <ul>
                <li>创建和分享精彩的内容</li>
                <li>与其他用户交流和互动</li>
                <li>探索各种有趣的主题和讨论</li>
            </ul>
            <p>点击下面的按钮开始您的 JackSite 之旅：</p>
            <p style='text-align: center;'>
                <a href='https://jacksite.com/dashboard' class='button'>进入我的控制台</a>
            </p>
            <p>如果您有任何问题或需要帮助，请随时联系我们的支持团队。</p>
            <p>祝您使用愉快！<br>JackSite 团队</p>
        </div>
        <div class='footer'>
            <p>此邮件由系统自动发送，请勿回复。</p>
        </div>
    </div>
</body>
</html>";
    }
    
    /// <summary>
    /// 生成通知邮件内容
    /// </summary>
    private string GenerateNotificationEmailBody(string username, string message)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>JackSite 通知</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #FF5722; color: white; padding: 10px; text-align: center; }}
        .content {{ padding: 20px; border: 1px solid #ddd; }}
        .message {{ background-color: #f9f9f9; padding: 15px; border-left: 4px solid #FF5722; margin: 15px 0; }}
        .button {{ display: inline-block; background-color: #FF5722; color: white; padding: 10px 20px; 
                  text-decoration: none; border-radius: 4px; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>JackSite 通知</h2>
        </div>
        <div class='content'>
            <p>您好，{username}！</p>
            <div class='message'>
                <p>{message}</p>
            </div>
            <p>如果您有任何问题或需要帮助，请随时联系我们的支持团队。</p>
            <p>谢谢！<br>JackSite 团队</p>
        </div>
        <div class='footer'>
            <p>此邮件由系统自动发送，请勿回复。</p>
        </div>
    </div>
</body>
</html>";
    }
}

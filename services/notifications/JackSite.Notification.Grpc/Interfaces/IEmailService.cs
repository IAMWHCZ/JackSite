using JackSite.Notification.Grpc.Entities;

namespace JackSite.Notification.Grpc.Interfaces;

/// <summary>
/// 邮件服务接口
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// 发送注册验证邮件
    /// </summary>
    Task<EmailLog> SendRegistrationVerificationEmailAsync(string email, string username, string verificationToken);
    
    /// <summary>
    /// 发送密码重置邮件
    /// </summary>
    Task<EmailLog> SendPasswordResetEmailAsync(string email, string username, string resetToken);
    
    /// <summary>
    /// 发送欢迎邮件
    /// </summary>
    Task<EmailLog> SendWelcomeEmailAsync(string email, string username);
    
    /// <summary>
    /// 发送通知邮件
    /// </summary>
    Task<EmailLog> SendNotificationEmailAsync(string email, string username, string subject, string message);
    
    /// <summary>
    /// 获取邮件状态
    /// </summary>
    Task<EmailLog?> GetEmailStatusAsync(Guid emailId);
}

using JackSite.Shared.EventBus.Events;

namespace JackSite.User.WebAPI.Domain.Events;

/// <summary>
/// 用户注册事件
/// </summary>
public class UserRegisteredEvent:EventBase
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public SnowflakeId UserId { get; }
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; }
    
    /// <summary>
    /// 电子邮件
    /// </summary>
    public string Email { get; }
    
    /// <summary>
    /// 注册时间
    /// </summary>
    public DateTime RegisteredAt { get; }
    
    /// <summary>
    /// 验证令牌
    /// </summary>
    public string VerificationToken { get; }
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public UserRegisteredEvent(SnowflakeId userId, string username, string email, string verificationToken)
    {
        UserId = userId;
        Username = username;
        Email = email;
        RegisteredAt = DateTime.UtcNow;
        VerificationToken = verificationToken;
    }
}
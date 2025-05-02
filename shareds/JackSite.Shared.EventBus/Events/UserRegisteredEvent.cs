namespace JackSite.Shared.EventBus.Events;

/// <summary>
/// 用户注册事件
/// </summary>
public class UserRegisteredEvent : IEvent
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; }
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; }
    
    /// <summary>
    /// 电子邮件
    /// </summary>
    public string Email { get; }
    
    /// <summary>
    /// 验证令牌
    /// </summary>
    public string VerificationToken { get; }
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public UserRegisteredEvent(string userId, string username, string email, string verificationToken, Guid id, DateTime creationDate)
    {
        UserId = userId;
        Username = username;
        Email = email;
        VerificationToken = verificationToken;
        Id = id;
        CreationDate = creationDate;
    }

    public Guid Id { get; }
    public DateTime CreationDate { get; }
}
using JackSite.Authentication.Entities.Clients;

namespace JackSite.Authentication.Entities.Users;

public class UserSession : Entity
{
    [Description("用户ID")]
    public Guid UserId { get; private set; }

    [Description("IP地址")]
    public string IpAddress { get; private set; } = null!;

    [Description("用户代理字符串")]
    public string? UserAgent { get; private set; }

    [Description("会话开始时间")]
    public DateTimeOffset StartTime { get; private set; }

    [Description("最后访问时间")]
    public DateTimeOffset LastAccess { get; private set; }

    [Description("会话过期时间")]
    public DateTimeOffset ExpiresAt { get; private set; }

    [Description("会话是否活跃")]
    public bool IsActive { get; private set; } = true;
    
    public virtual UserBasic User { get; private set; } = null!;
    public virtual ICollection<ClientSession> ClientSessions { get; private set; } = [];
}
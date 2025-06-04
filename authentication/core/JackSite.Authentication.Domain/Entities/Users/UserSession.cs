using JackSite.Authentication.Entities.Clients;

namespace JackSite.Authentication.Entities.Users;

public class UserSession : Entity
{
    public long UserId { get; private set; }
    public string IpAddress { get; private set; } = null!;
    public string? UserAgent { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime LastAccess { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    public virtual UserBasic User { get; private set; } = null!;
    public virtual ICollection<ClientSession> ClientSessions { get; private set; } = [];
}
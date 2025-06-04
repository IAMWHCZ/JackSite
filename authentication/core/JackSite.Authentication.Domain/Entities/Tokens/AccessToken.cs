using JackSite.Authentication.Entities.Clients;
using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Entities.Tokens;

public class AccessToken : Entity
{
    public string Token { get; private set; } = null!;
    public DateTime IssuedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public long? UserId { get; private set; }
    public long ClientId { get; private set; }
    public string? SessionId { get; private set; }
    public string Scopes { get; private set; } = null!; // 以空格分隔的作用域列表
    public bool Revoked { get; private set; }
    
    public virtual UserBasic? User { get; private set; }
    public virtual ClientBasic Client { get; private set; } = null!;
}
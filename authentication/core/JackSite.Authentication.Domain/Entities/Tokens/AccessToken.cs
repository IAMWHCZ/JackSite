using JackSite.Authentication.Entities.Clients;
using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Entities.Tokens;

public class AccessToken : Entity
{
    [Description("访问令牌")]
    public string Token { get; private set; } = null!;

    [Description("令牌发行时间")]
    public DateTimeOffset IssuedAt { get; private set; }

    [Description("令牌过期时间")]
    public DateTimeOffset ExpiresAt { get; private set; }

    [Description("用户ID")]
    public Guid? UserId { get; private set; }

    [Description("客户端ID")]
    public Guid ClientId { get; private set; }

    [Description("会话ID")]
    public string? SessionId { get; private set; }

    [Description("作用域列表")]
    public string Scopes { get; private set; } = null!; // 以空格分隔的作用域列表

    [Description("是否已撤销")]
    public bool Revoked { get; private set; }

    [Description("关联用户")]
    public virtual UserBasic? User { get; private set; }

    [Description("关联客户端")]
    public virtual ClientBasic Client { get; private set; } = null!;
}
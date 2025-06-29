using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Entities.Clients;

public class ClientSession : Entity
{
    [Description("用户会话ID")]
    public Guid UserSessionId { get; private set; }

    [Description("客户端ID")]
    public Guid ClientId { get; private set; }

    [Description("创建时间戳")]
    public DateTimeOffset Timestamp { get; private set; } = DateTimeOffset.Now;

    [Description("关联的用户会话")]
    public virtual UserSession UserSession { get; private set; } = null!;

    [Description("关联的客户端")]
    public virtual ClientBasic Client { get; private set; } = null!;

    private ClientSession() { } // 供 EF Core 使用

    public ClientSession(Guid userSessionId, Guid clientId)
    {
        UserSessionId = userSessionId;
        ClientId = clientId;
        Timestamp = DateTimeOffset.Now;
    }
}
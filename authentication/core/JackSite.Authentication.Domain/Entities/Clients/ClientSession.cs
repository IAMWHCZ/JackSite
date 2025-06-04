using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Entities.Clients;

public class ClientSession : Entity
{
    public long UserSessionId { get; private set; }
    public long ClientId { get; private set; }
    public DateTime Timestamp { get; private set; }
    
    public virtual UserSession UserSession { get; private set; } = null!;
    public virtual ClientBasic Client { get; private set; } = null!;
}
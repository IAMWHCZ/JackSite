namespace JackSite.Authentication.Entities.Clients;

public class ClientCorsOrigin : Entity
{
    public long ClientId { get; set; }
    public string Origin { get; private set; } = null!;
    public virtual ClientBasic Client { get; private set; } = null!;
    private ClientCorsOrigin() { } // 供 EF Core 使用
    
    public ClientCorsOrigin(long clientId, string origin)
    {
        ClientId = clientId;
        Origin = origin;
    }
}
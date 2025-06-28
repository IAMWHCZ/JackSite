namespace JackSite.Authentication.Entities.Clients;

public class ClientCorsOrigin : Entity
{
    public Guid ClientId { get; set; }
    public string Origin { get; private set; } = null!;
    public virtual ClientBasic Client { get; private set; } = null!;
    private ClientCorsOrigin() { } // 供 EF Core 使用
    
    public ClientCorsOrigin(Guid clientId, string origin)
    {
        ClientId = clientId;
        Origin = origin;
    }
}
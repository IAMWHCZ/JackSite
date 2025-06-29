namespace JackSite.Authentication.Entities.Clients;

public class ClientCorsOrigin : Entity
{
    [Description("客户端ID")]
    public Guid ClientId { get; set; }

    [Description("CORS源地址")]
    public string Origin { get; private set; } = null!;

    [Description("关联的客户端")]
    public virtual ClientBasic Client { get; private set; } = null!;

    private ClientCorsOrigin() { } // 供 EF Core 使用
    
    public ClientCorsOrigin(Guid clientId, string origin)
    {
        ClientId = clientId;
        Origin = origin;
    }
}
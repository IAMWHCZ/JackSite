namespace JackSite.Authentication.Entities.Clients;

public class ClientScope : Entity
{
    [Description("客户端ID")]
    public Guid ClientId { get; private set; }

    [Description("作用域名称")]
    public string ScopeName { get; private set; } = null!;

    [Description("关联的客户端")]
    public virtual ClientBasic Client { get; private set; } = null!;

    private ClientScope() { }

    public ClientScope(Guid clientId, string scope)
    {
        ClientId = clientId;
        ScopeName = scope;
    }
}
using JackSite.Authentication.Enums.Clients;

namespace JackSite.Authentication.Entities.Clients;

public class ClientRedirectUri : Entity
{
    [Description("客户端ID")]
    public Guid ClientId { get; private set; }

    [Description("重定向URI地址")]
    public string Uri { get; private set; } = null!;

    [Description("重定向类型")]
    public ClientRedircetType ClientRedircet { get; set; } = ClientRedircetType.SignIn;

    [Description("关联的客户端")]
    public virtual ClientBasic Client { get; private set; } = null!;
    
    private ClientRedirectUri() { }
    
    public ClientRedirectUri(Guid clientId, string uri, ClientRedircetType clientRedircet = ClientRedircetType.SignIn)
    {
        ClientId = clientId;
        Uri = uri;
        ClientRedircet = clientRedircet;
    }
}
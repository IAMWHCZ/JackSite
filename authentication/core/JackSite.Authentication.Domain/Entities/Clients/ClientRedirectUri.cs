namespace JackSite.Authentication.Entities.Clients;

public class ClientRedirectUri : Entity
{
    public long ClientId { get; private set; }
    public string Uri { get; private set; } = null!;
    
    public virtual ClientBasic Client { get; private set; } = null!;
    
    private ClientRedirectUri() { }
    
    public ClientRedirectUri(long clientId, string uri)
    {
        ClientId = clientId;
        Uri = uri;
    }
}
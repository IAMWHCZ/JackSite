namespace JackSite.Authentication.Entities.Clients;

public class ClientRedirectUri : Entity
{
    public Guid ClientId { get; private set; }
    public string Uri { get; private set; } = null!;
    
    public virtual ClientBasic Client { get; private set; } = null!;
    
    private ClientRedirectUri() { }
    
    public ClientRedirectUri(Guid clientId, string uri)
    {
        ClientId = clientId;
        Uri = uri;
    }
}
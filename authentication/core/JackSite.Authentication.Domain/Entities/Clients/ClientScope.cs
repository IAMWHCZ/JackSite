namespace JackSite.Authentication.Entities.Clients;


public class ClientScope : Entity
{
    public long ClientId { get; private set; }
    public string Scope { get; private set; } = null!;
    
    public virtual ClientBasic Client { get; private set; } = null!;
    
    private ClientScope() { }
    
    public ClientScope(long clientId, string scope)
    {
        ClientId = clientId;
        Scope = scope;
    }
}
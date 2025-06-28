namespace JackSite.Authentication.Entities.Clients;


public class ClientScope : Entity
{
    public Guid ClientId { get; private set; }
    public string Scope { get; private set; } = null!;
    
    public virtual ClientBasic Client { get; private set; } = null!;
    
    private ClientScope() { }
    
    public ClientScope(Guid clientId, string scope)
    {
        ClientId = clientId;
        Scope = scope;
    }
}
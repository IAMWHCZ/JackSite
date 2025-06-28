namespace JackSite.Authentication.Entities.Scopes;

public class ScopeClaim : Entity
{
    public Guid ScopeId { get; private set; }
    public string Claim { get; private set; } = null!;
    
    public virtual Scope Scope { get; private set; } = null!;
}
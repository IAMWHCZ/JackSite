namespace JackSite.Authentication.Entities.Scopes;

public class Scope : Entity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsDefault { get; private set; }
    
    public virtual ICollection<ScopeClaim> Claims { get; private set; } = [];
}
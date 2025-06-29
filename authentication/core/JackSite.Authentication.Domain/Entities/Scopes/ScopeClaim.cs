namespace JackSite.Authentication.Entities.Scopes;
public class ScopeClaim : Entity
{
    [Description("作用域ID")]
    public Guid ScopeId { get; private set; }

    [Description("声明内容")]
    public string Claim { get; private set; } = null!;

    [Description("关联的作用域")]
    public virtual Scope Scope { get; private set; } = null!;
}
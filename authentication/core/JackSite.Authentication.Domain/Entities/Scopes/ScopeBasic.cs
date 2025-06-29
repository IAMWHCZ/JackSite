namespace JackSite.Authentication.Entities.Scopes;

public class Scope : Entity
{
    [Description("作用域名称")]
    public string Name { get; private set; } = null!;

    [Description("作用域描述")]
    public string? Description { get; private set; }

    [Description("是否默认作用域")]
    public bool IsDefault { get; private set; }

    [Description("作用域声明集合")]
    public virtual ICollection<ScopeClaim> Claims { get; private set; } = [];
}
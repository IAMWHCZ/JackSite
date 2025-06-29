namespace JackSite.Authentication.Entities.Roles;

public class Role : Entity
{
    [Description("角色名称")]
    public string RoleName { get; set; } = string.Empty;

    [Description("角色描述")]
    public string? Description { get; set; }

    [Description("用户组集合")]
    public virtual ICollection<UserGroup>? UserGroups { get; set; }

    [Description("角色引用集合")]
    public virtual ICollection<RoleReference>? RoleReferences { get; set; }
}
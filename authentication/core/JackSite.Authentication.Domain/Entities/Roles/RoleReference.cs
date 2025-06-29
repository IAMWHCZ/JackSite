namespace JackSite.Authentication.Entities.Roles;

public class RoleReference : Entity
{
    [Description("角色ID")]
    public Guid RoleId { get; set; }

    [Description("用户组ID")]
    public Guid GroupId { get; set; }
}
namespace JackSite.Authentication.Entities.Roles;

public class Role:Entity
{
    public string RoleName { get; set; } = string.Empty;
    
    public string? Description { get; set; }

    public virtual ICollection<UserGroup>? UserGroups { get; set; }

    public virtual ICollection<RoleReference>? RoleReferences { get; set; }
}
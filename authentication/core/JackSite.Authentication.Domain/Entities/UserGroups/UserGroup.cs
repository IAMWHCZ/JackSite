namespace JackSite.Authentication.Entities.UserGroups;

public class UserGroup:Entity
{
    public string GroupName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public virtual ICollection<UserGroupReference>? UserGroupReferences { get; set; }
}
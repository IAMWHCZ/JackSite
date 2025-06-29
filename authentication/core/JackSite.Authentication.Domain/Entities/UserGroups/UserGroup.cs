namespace JackSite.Authentication.Entities.UserGroups;

public class UserGroup : Entity
{
    [Description("用户组名称")]
    public string GroupName { get; set; } = string.Empty;

    [Description("用户组描述")]
    public string? Description { get; set; }

    [Description("用户组引用集合")]
    public virtual ICollection<UserGroupReference>? UserGroupReferences { get; set; }
}
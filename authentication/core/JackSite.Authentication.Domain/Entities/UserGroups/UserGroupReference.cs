using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Entities.UserGroups;

public class UserGroupReference : Entity
{
    [Description("用户ID")]
    public Guid UserId { get; set; }
    
    [Description("用户组ID")]
    public Guid UserGroupId { get; set; }
    
    [Description("关联用户")]
    public virtual UserBasic User { get; set; } = null!;
    
    [Description("关联用户组")]
    public virtual UserGroup UserGroup { get; set; } = null!;
}
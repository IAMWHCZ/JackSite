using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Entities.UserGroups;

public class UserGroupReference:Entity
{
    public Guid UserId { get; set; }
    
    public Guid UserGroupId { get; set; }
    
    public virtual UserBasic User { get; set; } = null!;
    
    public virtual UserGroup UserGroup { get; set; } = null!;
}
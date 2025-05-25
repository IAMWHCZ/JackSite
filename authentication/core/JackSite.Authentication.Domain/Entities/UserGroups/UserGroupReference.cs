using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Entities.UserGroups;

public class UserGroupReference:Entity
{
    public long UserId { get; set; }
    
    public long UserGroupId { get; set; }
    
    public virtual UserBasic User { get; set; } = null!;
    
    public virtual UserGroup UserGroup { get; set; } = null!;
}
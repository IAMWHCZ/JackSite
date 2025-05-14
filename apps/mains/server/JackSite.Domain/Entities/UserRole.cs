namespace JackSite.Domain.Entities;

[Table("UserRoles")]
public class UserRole : Entity
{
    [Required]
    public long UserId { get; private set; }
    
    [Required]
    public long RoleId { get; private set; }
    
    public virtual UserBasic UserBasic { get; private set; } = null!;
    
    public virtual Role Role { get; private set; } = null!;
    
    // 供EF Core使用的私有构造函数
    private UserRole() { }
    
    // 领域构造函数
    public UserRole(long userId, long roleId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero", nameof(userId));
            
        if (roleId <= 0)
            throw new ArgumentException("Role ID must be greater than zero", nameof(roleId));
            
        UserId = userId;
        RoleId = roleId;
    }
}
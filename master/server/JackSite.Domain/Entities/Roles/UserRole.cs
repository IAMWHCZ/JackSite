namespace JackSite.Domain.Entities.Roles;

public class UserRole : Entity
{
    [Required]
    public long UserId { get; private set; }
    
    [Required]
    public long RoleId { get; private set; }
    
    // 导航属性
    public UserBasic User { get; private set; } = null!;
    public Role Role { get; private set; } = null!;
    
    // 私有构造函数供EF Core使用
    private UserRole() { }
    
    // 领域构造函数
    public UserRole(long userId, long roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}
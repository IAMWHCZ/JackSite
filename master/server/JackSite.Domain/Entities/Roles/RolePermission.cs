using JackSite.Domain.Entities.Permissions;

namespace JackSite.Domain.Entities.Roles;

public class RolePermission : Entity
{
    [Required]
    public long RoleId { get; private set; }
    
    [Required]
    public long PermissionId { get; private set; }
    
    // 导航属性
    public Role Role { get; private set; } = null!;
    public Permission Permission { get; private set; } = null!;
    
    // 私有构造函数供EF Core使用
    private RolePermission() { }
    
    // 领域构造函数
    public RolePermission(long roleId, long permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
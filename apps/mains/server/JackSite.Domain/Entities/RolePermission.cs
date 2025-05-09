namespace JackSite.Domain.Entities;

public  class RolePermission : BaseEntity
{
    [Required]
    public long RoleId { get; set; }
    
    [Required]
    public long PermissionId { get; set; }
    
    public virtual Role Role { get; set; } = null!;
    
    public virtual Permission Permission { get; set; } = null!;

    // 供EF Core使用的私有构造函数
    private RolePermission() { }
    
    // 领域构造函数
    public RolePermission(long roleId, long permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
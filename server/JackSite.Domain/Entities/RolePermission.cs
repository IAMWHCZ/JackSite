using JackSite.Domain.Base;

namespace JackSite.Domain.Entities;

public class RolePermission : BaseEntity
{
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
    
    // 导航属性
    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}
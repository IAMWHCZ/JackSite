namespace JackSite.Domain.Entities;

public class Permission : BaseEntity, ISoftDeletable
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    
    // 导航属性
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
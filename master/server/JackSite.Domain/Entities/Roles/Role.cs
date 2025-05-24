namespace JackSite.Domain.Entities.Roles;

public class Role : Entity, ISoftDeletable, IAggregateRoot
{
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Name { get; private set; } = string.Empty;
    
    [MaxLength(200)]
    [Column(TypeName = "varchar(200)")]
    public string? Description { get; private set; }
    
    [Required]
    public bool IsDefault { get; private set; } = false;
    
    [Required]
    public bool IsSystem { get; private set; } = false;
    
    [Required]
    public bool IsActive { get; private set; } = true;
    
    [Required]
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedOnUtc { get; set; }
    
    // 导航属性 - 用户角色关系
    public ICollection<UserRole> UserRoles { get; private set; } = [];
    
    // 导航属性 - 角色权限关系
    public ICollection<RolePermission> RolePermissions { get; private set; } = [];
    
    // 私有构造函数供EF Core使用
    private Role() { }
    
    // 领域构造函数
    public Role(string name, string? description = null, bool isDefault = false, bool isSystem = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty", nameof(name));
            
        Name = name;
        Description = description;
        IsDefault = isDefault;
        IsSystem = isSystem;
    }
    
    // 领域行为
    public void UpdateInfo(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty", nameof(name));
            
        Name = name;
        Description = description;
    }
    
    public void SetAsDefault(bool isDefault) => IsDefault = isDefault;
    
    public void SetAsSystem(bool isSystem) => IsSystem = isSystem;
    
    public void Activate() => IsActive = true;
    
    public void Deactivate() => IsActive = false;
    
    // ISoftDeletable接口实现
    public void Delete()
    {
        if (IsDeleted)
            return;
            
        IsDeleted = true;
        DeletedOnUtc = DateTime.UtcNow;
    }
    
    public void Restore()
    {
        if (!IsDeleted)
            return;
            
        IsDeleted = false;
        DeletedOnUtc = null;
    }
}

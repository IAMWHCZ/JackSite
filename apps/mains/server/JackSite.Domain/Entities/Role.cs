namespace JackSite.Domain.Entities;
public class Role : Entity, ISoftDeletable, IAggregateRoot
{
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Name { get; private set; } = string.Empty;
    
    [MaxLength(200)]
    [Column(TypeName = "varchar(200)")]
    public string Description { get; private set; } = string.Empty;
    
    [Required]
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedOnUtc { get; set; }
    
    // 导航属性
    private readonly List<UserRole> _userRoles = [];
    public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    
    private readonly List<RolePermission> _rolePermissions = [];
    public virtual IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();
    
    // 供EF Core使用的私有构造函数
    private Role() { }
    
    // 领域构造函数
    public Role(string name, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty", nameof(name));
            
        Name = name;
        Description = description;
    }
    
    // 领域行为
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Role name cannot be empty", nameof(newName));
            
        Name = newName;
    }
    
    public void UpdateDescription(string newDescription)
    {
        Description = newDescription ?? string.Empty;
    }
    
    public void AddPermission(Permission permission)
    {
        ArgumentNullException.ThrowIfNull(permission);

        if (_rolePermissions.Any(rp => rp.PermissionId == permission.Id))
            return; // 角色已拥有该权限
            
        _rolePermissions.Add(new RolePermission(Id, permission.Id));
    }
    
    public void RemovePermission(long permissionId)
    {
        var rolePermission = _rolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
        if (rolePermission != null)
            _rolePermissions.Remove(rolePermission);
    }
    
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
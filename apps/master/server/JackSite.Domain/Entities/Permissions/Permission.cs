namespace JackSite.Domain.Entities.Permissions;

public class Permission : Entity
{
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Name { get; private set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Code { get; private set; } = string.Empty;
    
    [MaxLength(200)]
    [Column(TypeName = "varchar(200)")]
    public string? Description { get; private set; }
    
    [Required]
    public bool IsSystem { get; private set; } = false;
    
    // 导航属性
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
    
    // 私有构造函数供EF Core使用
    private Permission() { }
    
    // 领域构造函数
    public Permission(string name, string code, string? description = null, bool isSystem = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Permission name cannot be empty", nameof(name));
            
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Permission code cannot be empty", nameof(code));
            
        Name = name;
        Code = code;
        Description = description;
        IsSystem = isSystem;
    }
}
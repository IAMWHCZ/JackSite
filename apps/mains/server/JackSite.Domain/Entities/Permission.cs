namespace JackSite.Domain.Entities;

public sealed class Permission : Entity, ISoftDeletable
{
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Name { get; private set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Code { get; private set; } = string.Empty;
    
    [MaxLength(200)]
    
    [Column(TypeName = "varchar(200)")]
    public string Description { get; private set; } = string.Empty;
    [Required]
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    
    private readonly List<RolePermission> _rolePermissions = [];
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();
    
    // 供EF Core使用的私有构造函数
    private Permission() { }
    
    // 领域构造函数
    public Permission(string name, string code, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Permission name cannot be empty", nameof(name));
            
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Permission code cannot be empty", nameof(code));
            
        Name = name;
        Code = code;
        Description = description;
    }
    
    // 领域行为
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Permission name cannot be empty", nameof(newName));
            
        Name = newName;
    }
    
    public void UpdateCode(string newCode)
    {
        if (string.IsNullOrWhiteSpace(newCode))
            throw new ArgumentException("Permission code cannot be empty", nameof(newCode));
            
        Code = newCode;
    }
    
    public void UpdateDescription(string newDescription)
    {
        Description = newDescription ?? string.Empty;
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
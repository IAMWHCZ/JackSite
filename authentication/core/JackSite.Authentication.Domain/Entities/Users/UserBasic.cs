namespace JackSite.Authentication.Entities.Users;

public class UserBasic : Entity, ISoftDeletable, IAggregateRoot
{
    // 基本属性
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Username { get; private set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Email { get; private set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string PasswordHash { get; private set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Salt { get; private set; } = string.Empty;

    [Required] public bool IsActive { get; private set; } = true;

    [Required] public bool IsDeleted { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    // 新增属性
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? PhoneNumber { get; private set; }

    [Required] public bool PhoneNumberConfirmed { get; private set; }

    [Required] public bool EmailConfirmed { get; private set; }

    [MaxLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string? AvatarUrl { get; private set; }

    [Required] public DateTime LastLoginTime { get; private set; }

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? LastLoginIp { get; private set; }

    [Required] public int LoginCount { get; private set; }

    [Required] public DateTime RegisterTime { get; private set; } = DateTime.UtcNow;

    [Required] public UserStatus Status { get; private set; } = UserStatus.Normal;

    public virtual UserProfile? UserProfile { get; private set; }

    public virtual UserSettings? UserSettings { get; private set; }

    // 供EF Core使用的私有构造函数
    private UserBasic()
    {
    }

    // 领域构造函数
    public UserBasic(string username, string email, string passwordHash, string salt)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("Salt cannot be empty", nameof(salt));

        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Salt = salt;
        RegisterTime = DateTime.UtcNow;
        LastLoginTime = DateTime.UtcNow;
    }

    // 领域行为
    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email cannot be empty", nameof(newEmail));

        Email = newEmail;
    }

    public void UpdatePassword(string newPasswordHash, string newSalt)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(newPasswordHash));

        if (string.IsNullOrWhiteSpace(newSalt))
            throw new ArgumentException("Salt cannot be empty", nameof(newSalt));

        PasswordHash = newPasswordHash;
        Salt = newSalt;
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
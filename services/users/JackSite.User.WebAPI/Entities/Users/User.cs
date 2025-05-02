namespace JackSite.User.WebAPI.Entities.Users;

/// <summary>
/// 用户实体
/// </summary>
public class User : SoftDeleteEntityBase<SnowflakeId>
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    /// <summary>
    /// 电子邮件
    /// </summary>
    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    /// <summary>
    /// 电子邮件是否已验证
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// 密码哈希
    /// </summary>
    [Required]
    [StringLength(256)]
    [JsonIgnore]
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// 安全戳
    /// </summary>
    [Required]
    [StringLength(100)]
    [JsonIgnore]
    public string SecurityStamp { get; set; } = null!;

    /// <summary>
    /// 手机号码
    /// </summary>
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 手机号码是否已验证
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// 账号状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Enabled;

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; } = UserType.Local;

    /// <summary>
    /// 用户个人资料
    /// </summary>
    public UserProfile? Profile { get; set; }

    /// <summary>
    /// 用户安全信息
    /// </summary>
    public UserSecurity? Security { get; set; }

    /// <summary>
    /// 用户偏好设置
    /// </summary>

    public UserPreference? Preference { get; set; }
}
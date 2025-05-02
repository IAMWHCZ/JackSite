namespace JackSite.User.WebAPI.Entities.Users;

public class UserSecurity : EntityBase<SnowflakeId>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required]
    public SnowflakeId UserId { get; set; }
    
    /// <summary>
    /// 是否启用双因素认证
    /// </summary>
    public bool TwoFactorEnabled { get; set; }
    
    /// <summary>
    /// 锁定结束时间
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; set; }
    
    /// <summary>
    /// 是否启用锁定
    /// </summary>
    public bool LockoutEnabled { get; set; }
    
    /// <summary>
    /// 登录失败次数
    /// </summary>
    public int AccessFailedCount { get; set; }
    
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTimeOffset? LastLoginTime { get; set; }
    
    /// <summary>
    /// 最后登录IP
    /// </summary>
    [StringLength(50)]
    public string? LastLoginIp { get; set; }
    
    /// <summary>
    /// 注册IP
    /// </summary>
    [StringLength(50)]
    public string? RegisterIp { get; set; }
    
    /// <summary>
    /// 刷新令牌
    /// </summary>
    [StringLength(256)]
    [JsonIgnore]
    public string? RefreshToken { get; set; }
    
    /// <summary>
    /// 刷新令牌过期时间
    /// </summary>
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
    
    /// <summary>
    /// 密码重置令牌
    /// </summary>
    [StringLength(256)]
    [JsonIgnore]
    public string? PasswordResetToken { get; set; }
    
    /// <summary>
    /// 密码重置令牌过期时间
    /// </summary>
    public DateTimeOffset? PasswordResetTokenExpiryTime { get; set; }
    
    /// <summary>
    /// 最后密码修改时间
    /// </summary>
    public DateTimeOffset? LastPasswordChangedTime { get; set; }
    
    /// <summary>
    /// 用户
    /// </summary>
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
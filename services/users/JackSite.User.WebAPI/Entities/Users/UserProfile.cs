namespace JackSite.User.WebAPI.Entities.Users;

public class UserProfile : EntityBase<SnowflakeId>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required]
    public SnowflakeId UserId { get; set; }
    
    /// <summary>
    /// 姓名
    /// </summary>
    [StringLength(50)]
    public string? RealName { get; set; }
    
    /// <summary>
    /// 昵称
    /// </summary>
    [StringLength(50)]
    public string? NickName { get; set; }
    
    /// <summary>
    /// 头像URL
    /// </summary>
    [StringLength(500)]
    public string? AvatarUrl { get; set; }
    
    /// <summary>
    /// 性别（0-未知，1-男，2-女）
    /// </summary>
    public UserGender Gender { get; set; }
    
    /// <summary>
    /// 生日
    /// </summary>
    public DateOnly? Birthday { get; set; }
    
    /// <summary>
    /// 个人简介
    /// </summary>
    [StringLength(500)]
    public string? Biography { get; set; }
    
    /// <summary>
    /// 所在地区
    /// </summary>
    [StringLength(100)]
    public string? Location { get; set; }
    
    /// <summary>
    /// 个人网站
    /// </summary>
    [StringLength(200)]
    public string? Website { get; set; }
    
    /// <summary>
    /// 社交媒体账号（JSON格式）
    /// </summary>
    public string? SocialMedia { get; set; }
    
    /// <summary>
    /// 兴趣爱好
    /// </summary>
    [StringLength(500)]
    public string? Interests { get; set; }
    
    /// <summary>
    /// 扩展属性（JSON格式）
    /// </summary>
    public string? ExtendedAttributes { get; set; }
    
    /// <summary>
    /// 用户
    /// </summary>
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
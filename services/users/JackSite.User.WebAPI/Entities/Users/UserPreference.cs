namespace JackSite.User.WebAPI.Entities.Users;

/// <summary>
/// 用户偏好设置
/// </summary>
public class UserPreference : EntityBase<SnowflakeId>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public SnowflakeId UserId { get; set; }
    
    /// <summary>
    /// 主题
    /// </summary>
    public string? Theme { get; set; }
    
    /// <summary>
    /// 语言
    /// </summary>
    public string? Language { get; set; }
    
    /// <summary>
    /// 时区
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// 日期格式
    /// </summary>
    public string? DateFormat { get; set; }
    
    /// <summary>
    /// 时间格式
    /// </summary>
    public string? TimeFormat { get; set; }
    
    /// <summary>
    /// 首页
    /// </summary>
    public string? HomePage { get; set; }
    
    /// <summary>
    /// 通知设置（JSON格式）
    /// </summary>
    public string? NotificationSettings { get; set; }
    
    /// <summary>
    /// 其他设置（JSON格式）
    /// </summary>
    public string? OtherSettings { get; set; }
    
    /// <summary>
    /// 用户
    /// </summary>
    public User User { get; set; } = null!;
}
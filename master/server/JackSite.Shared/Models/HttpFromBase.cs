namespace JackSite.Shared.Models;

public class HttpFromBase
{
    /// <summary>
    /// 用户语言偏好
    /// </summary>
    public string Language { get; set; } = "en-US";
    
    /// <summary>
    /// 用户时区
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// 用户ID（如果已登录）
    /// </summary>
    public string? UserId { get; set; }
    
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }
    
    
    /// <summary>
    /// 用户邮箱
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// 请求时间
    /// </summary>
    public DateTime AccessTime { get; set; } = DateTime.UtcNow;

}
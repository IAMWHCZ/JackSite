namespace JackSite.Notification.Grpc.Options;

/// <summary>
/// 邮件配置选项
/// </summary>
public class EmailOptions
{
    /// <summary>
    /// SMTP服务器地址
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;
    
    /// <summary>
    /// SMTP端口
    /// </summary>
    public int SmtpPort { get; set; } = 587;
    
    /// <summary>
    /// 是否使用SSL
    /// </summary>
    public bool UseSsl { get; set; } = true;
    
    /// <summary>
    /// 发件人邮箱
    /// </summary>
    public string SenderEmail { get; set; } = string.Empty;
    
    /// <summary>
    /// 发件人显示名称
    /// </summary>
    public string SenderName { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
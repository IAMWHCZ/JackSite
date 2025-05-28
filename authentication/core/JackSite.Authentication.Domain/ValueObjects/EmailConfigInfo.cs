namespace JackSite.Authentication.ValueObjects;

/// <summary>
/// 邮箱配置信息
/// </summary>
public class EmailConfigInfo
{
    /// <summary>
    /// SMTP服务器地址
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// SMTP端口
    /// </summary>
    public int SmtpPort { get; set; }

    /// <summary>
    /// 是否启用SSL
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// 邮箱账号
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱密码或授权码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 发件人邮箱
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// 发件人显示名称
    /// </summary>
    public string? DisplayName { get; set; }
}


namespace JackSite.Shared.EventBus.Options;

/// <summary>
/// 事件总线配置选项
/// </summary>
public class EventBusOptions
{
    /// <summary>
    /// 主机名
    /// </summary>
    public string HostName { get; set; } = "localhost";
    
    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; } = 5672;
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = "guest";
    
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = "guest";
    
    /// <summary>
    /// 虚拟主机
    /// </summary>
    public string VirtualHost { get; set; } = "/";
    
    /// <summary>
    /// 重试次数
    /// </summary>
    public int RetryCount { get; set; } = 5;
    
    /// <summary>
    /// 服务名称（用于队列命名）
    /// </summary>
    public string ServiceName { get; set; } = "JackSite";
    
    /// <summary>
    /// 是否使用SSL
    /// </summary>
    public bool UseSSL { get; set; } = false;
}
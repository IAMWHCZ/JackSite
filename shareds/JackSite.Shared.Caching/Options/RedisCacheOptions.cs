namespace JackSite.Shared.Caching.Options;

/// <summary>
/// Redis 缓存配置选项
/// </summary>
public class RedisCacheOptions
{
    /// <summary>
    /// Redis 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = "localhost:6379";
    
    /// <summary>
    /// 实例名称
    /// </summary>
    public string InstanceName { get; set; } = "JackSite:";
    
    /// <summary>
    /// 默认过期时间（秒）
    /// </summary>
    public int DefaultExpirationSeconds { get; set; } = 300;
}
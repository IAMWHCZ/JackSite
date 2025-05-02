namespace JackSite.Shared.MongoDB.Configuration;

/// <summary>
/// MongoDB 配置
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    
    /// <summary>
    /// 数据库名称
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否启用分片
    /// </summary>
    public bool EnableSharding { get; set; } = false;
    
    /// <summary>
    /// 是否启用事务
    /// </summary>
    public bool EnableTransactions { get; set; } = false;
    
    /// <summary>
    /// 最大连接池大小
    /// </summary>
    public int MaxConnectionPoolSize { get; set; } = 100;
    
    /// <summary>
    /// 连接超时（毫秒）
    /// </summary>
    public int ConnectionTimeoutMs { get; set; } = 30000;
    
    /// <summary>
    /// 服务器选择超时（毫秒）
    /// </summary>
    public int ServerSelectionTimeoutMs { get; set; } = 30000;
    
    /// <summary>
    /// 套接字超时（毫秒）
    /// </summary>
    public int SocketTimeoutMs { get; set; } = 30000;
    
    /// <summary>
    /// 是否使用直接连接
    /// </summary>
    public bool DirectConnection { get; set; }
    
    /// <summary>
    /// 是否启用重试写入
    /// </summary>
    public bool RetryWrites { get; set; } = true;
    
    /// <summary>
    /// 是否启用重试读取
    /// </summary>
    public bool RetryReads { get; set; } = true;
}
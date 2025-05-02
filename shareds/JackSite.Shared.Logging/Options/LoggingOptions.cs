namespace JackSite.Shared.Logging.Options;

/// <summary>
/// 日志配置选项
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public string ApplicationName { get; set; } = "JackSite";
    
    /// <summary>
    /// 最小日志级别
    /// </summary>
    public string MinimumLevel { get; set; } = "Information";
    
    /// <summary>
    /// 覆盖特定命名空间的日志级别
    /// </summary>
    public Dictionary<string, string> Override { get; set; } = new();
    
    /// <summary>
    /// 是否启用控制台日志
    /// </summary>
    public bool EnableConsole { get; set; } = true;
    
    /// <summary>
    /// 是否启用文件日志
    /// </summary>
    public bool EnableFile { get; set; } = true;
    
    /// <summary>
    /// 文件日志路径
    /// </summary>
    public string FilePath { get; set; } = "logs/log-.txt";
    
    /// <summary>
    /// 文件日志滚动间隔
    /// </summary>
    public string FileRollingInterval { get; set; } = "Day";
    
    /// <summary>
    /// 是否启用 Seq 日志
    /// </summary>
    public bool EnableSeq { get; set; } = false;
    
    /// <summary>
    /// Seq 服务器 URL
    /// </summary>
    public string SeqServerUrl { get; set; } = "http://localhost:5341";
    
    /// <summary>
    /// Seq API 密钥
    /// </summary>
    public string SeqApiKey { get; set; } = "";

    // 添加数据库日志配置
    /// <summary>
    /// 是否启用数据库日志
    /// </summary>
    public bool EnableDatabase { get; set; } = false;

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string DatabaseConnectionString { get; set; } = "";

    /// <summary>
    /// 数据库日志表名
    /// </summary>
    public string DatabaseTableName { get; set; } = "Logs";

    /// <summary>
    /// 自定义日志表列映射
    /// </summary>
    public Dictionary<string, string> DatabaseColumnMap { get; set; } = new();

    /// <summary>
    /// 是否使用自定义日志实体
    /// </summary>
    public bool UseCustomLogEntity { get; set; } = false;
}

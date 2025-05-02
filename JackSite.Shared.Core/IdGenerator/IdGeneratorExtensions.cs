namespace JackSite.Shared.Core.IdGenerator;

/// <summary>
/// ID 生成器扩展方法
/// </summary>
public static class IdGeneratorExtensions
{
    private static readonly SnowflakeIdGenerator DefaultGenerator = 
        IdGeneratorFactory.GetSnowflakeGenerator("default");
    
    /// <summary>
    /// 生成新的雪花 ID
    /// </summary>
    /// <returns>雪花 ID</returns>
    public static long NewId()
    {
        return DefaultGenerator.NextId();
    }
    
    /// <summary>
    /// 生成新的雪花 ID 结构体
    /// </summary>
    /// <returns>雪花 ID 结构体</returns>
    public static SnowflakeId NewSnowflakeId()
    {
        return SnowflakeId.NewId();
    }
    
    /// <summary>
    /// 生成新的雪花 ID 字符串
    /// </summary>
    /// <returns>雪花 ID 字符串</returns>
    public static string NewIdString()
    {
        return DefaultGenerator.NextId().ToString();
    }
    
    /// <summary>
    /// 从雪花 ID 中提取时间戳
    /// </summary>
    /// <param name="id">雪花 ID</param>
    /// <returns>时间戳（毫秒）</returns>
    public static long GetTimestamp(long id)
    {
        const long twepoch = 1288834974657L;
        const int timestampLeftShift = 22;
        
        return (id >> timestampLeftShift) + twepoch;
    }
    
    /// <summary>
    /// 从雪花 ID 中提取生成时间
    /// </summary>
    /// <param name="id">雪花 ID</param>
    /// <returns>生成时间</returns>
    public static DateTimeOffset GetDateTime(long id)
    {
        var timestamp = GetTimestamp(id);
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
    }
}
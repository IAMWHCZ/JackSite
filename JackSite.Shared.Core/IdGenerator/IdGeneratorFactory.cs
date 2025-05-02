namespace JackSite.Shared.Core.IdGenerator;

/// <summary>
/// ID 生成器工厂
/// </summary>
public class IdGeneratorFactory
{
    private static readonly Dictionary<string, SnowflakeIdGenerator> Generators = new();
    private static readonly object Lock = new();
    
    /// <summary>
    /// 获取雪花 ID 生成器
    /// </summary>
    /// <param name="name">生成器名称</param>
    /// <param name="workerId">机器 ID</param>
    /// <param name="datacenterId">数据中心 ID</param>
    /// <returns>雪花 ID 生成器</returns>
    public static SnowflakeIdGenerator GetSnowflakeGenerator(string name, long workerId = 1, long datacenterId = 1)
    {
        if (Generators.TryGetValue(name, out var generator))
            return generator;
            
        lock (Lock)
        {
            if (Generators.TryGetValue(name, out generator))
                return generator;
                
            generator = new SnowflakeIdGenerator(workerId, datacenterId);
            Generators[name] = generator;
            
            return generator;
        }
    }
}
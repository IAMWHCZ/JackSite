namespace JackSite.Shared.Core.Extensions;

/// <summary>
/// 服务集合扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加雪花 ID 生成器
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="workerId">机器 ID</param>
    /// <param name="datacenterId">数据中心 ID</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddSnowflakeIdGenerator(
        this IServiceCollection services,
        long workerId = 1,
        long datacenterId = 1)
    {
        // 注册默认生成器
        var generator = IdGeneratorFactory.GetSnowflakeGenerator("default", workerId, datacenterId);
        services.AddSingleton(generator);
        
        return services;
    }
}
namespace JackSite.Shared.Logging.Extensions;

/// <summary>
/// 服务集合扩展
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加 JackSite 日志服务
    /// </summary>
    public static IServiceCollection AddJackSiteLogging(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionPath = "Logging")
    {
        // 配置日志选项
        var loggingOptions = new LoggingOptions();
        configuration.GetSection(configSectionPath).Bind(loggingOptions);
        services.Configure<LoggingOptions>(configuration.GetSection(configSectionPath));
        
        return services;
    }
}
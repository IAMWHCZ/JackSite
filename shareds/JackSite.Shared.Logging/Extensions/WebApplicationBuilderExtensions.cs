namespace JackSite.Shared.Logging.Extensions;

/// <summary>
/// WebApplicationBuilder 扩展
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// 添加 JackSite Serilog
    /// </summary>
    public static WebApplicationBuilder AddJackSiteSerilog(
        this WebApplicationBuilder builder,
        string configSectionPath = "Logging")
    {
        // 配置日志选项
        var loggingOptions = new LoggingOptions();
        builder.Configuration.GetSection(configSectionPath).Bind(loggingOptions);
        
        // 创建 Serilog 配置
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Is(GetLogEventLevel(loggingOptions.MinimumLevel))
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithExceptionDetails()
            .Enrich.With<ActivityEnricher>();
            
        // 添加应用名称
        if (!string.IsNullOrEmpty(loggingOptions.ApplicationName))
        {
            loggerConfiguration.Enrich.WithProperty("Application", loggingOptions.ApplicationName);
        }
        
        // 配置覆盖的日志级别
        foreach (var (source, level) in loggingOptions.Override)
        {
            loggerConfiguration.MinimumLevel.Override(source, GetLogEventLevel(level));
        }
        
        // 添加控制台日志
        if (loggingOptions.EnableConsole)
        {
            loggerConfiguration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code);
        }
        
        // 添加文件日志
        if (loggingOptions.EnableFile)
        {
            var rollingInterval = Enum.Parse<RollingInterval>(loggingOptions.FileRollingInterval);
            
            loggerConfiguration.WriteTo.File(
                path: loggingOptions.FilePath,
                rollingInterval: rollingInterval,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
        }
        
        // 添加 Seq 日志
        if (loggingOptions.EnableSeq && !string.IsNullOrEmpty(loggingOptions.SeqServerUrl))
        {
            loggerConfiguration.WriteTo.Seq(
                serverUrl: loggingOptions.SeqServerUrl,
                apiKey: string.IsNullOrEmpty(loggingOptions.SeqApiKey) ? null : loggingOptions.SeqApiKey);
        }
        
        // 添加数据库日志
        if (loggingOptions.EnableDatabase && !string.IsNullOrEmpty(loggingOptions.DatabaseConnectionString))
        {
            loggerConfiguration.WriteTo.MSSqlServer(
                connectionString: loggingOptions.DatabaseConnectionString,
                tableName: loggingOptions.DatabaseTableName,
                autoCreateSqlTable: true);
        }
        
        // 设置 Serilog 为默认日志提供程序
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(loggerConfiguration.CreateLogger(), dispose: true);
        
        return builder;
    }
    
    /// <summary>
    /// 获取日志事件级别
    /// </summary>
    private static LogEventLevel GetLogEventLevel(string level)
    {
        return level?.ToLower() switch
        {
            "verbose" => LogEventLevel.Verbose,
            "debug" => LogEventLevel.Debug,
            "information" => LogEventLevel.Information,
            "warning" => LogEventLevel.Warning,
            "error" => LogEventLevel.Error,
            "fatal" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }
}
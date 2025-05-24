using Microsoft.Extensions.Hosting;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
namespace JackSite.Infrastructure.Logging;

public static class SerilogExtensions
{
    /// <summary>
    /// 配置 Serilog
    /// </summary>
    /// <param name="builder">Web 应用程序构建器</param>
    /// <param name="configuration">配置</param>
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        // 确保日志目录存在
        var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        
        // 从配置文件中读取 Serilog 配置
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("ApplicationVersion", typeof(SerilogExtensions).Assembly.GetName().Version?.ToString() ?? "1.0.0")
            .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName);
        
        // 在开发环境中使用彩色控制台
        if (builder.Environment.IsDevelopment())
        {
            loggerConfiguration
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code);
        }
        
        // 添加结构化JSON日志文件（适用于日志分析工具）
        loggerConfiguration.WriteTo.File(
            new CompactJsonFormatter(),
            Path.Combine(logDirectory, "jacksite-structured-.json"),
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7);
        
        // 仅在非开发环境或明确配置时添加数据库日志接收器
        var enableDbLogging = configuration.GetValue<bool>("Serilog:EnableDatabaseLogging");
        var isDevelopment = builder.Environment.IsDevelopment();
        
        if (!isDevelopment || enableDbLogging)
        {
            try
            {
                // 使用一个临时的服务提供者，避免在配置阶段构建完整的服务提供者
                var tempServiceProvider = builder.Services.BuildServiceProvider();
                loggerConfiguration.WriteTo.Sink(new PostgreSqlSink(tempServiceProvider));
            }
            catch (Exception ex)
            {
                // 如果添加数据库接收器失败，记录错误但继续配置其他接收器
                Console.WriteLine($"无法配置数据库日志接收器: {ex.Message}");
            }
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        // 记录应用程序启动
        Log.Information("配置 Serilog 日志系统完成");
        
        builder.Host.UseSerilog();

        return builder;
    }
}
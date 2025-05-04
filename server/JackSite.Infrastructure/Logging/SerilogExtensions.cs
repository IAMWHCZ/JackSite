using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

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
        // 从配置文件中读取 Serilog 配置
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration);

        loggerConfiguration.WriteTo.Sink(
            new PostgreSqlSink(
                builder.Services.BuildServiceProvider(),
                "JackSite.Http"));

        Log.Logger = loggerConfiguration.CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}
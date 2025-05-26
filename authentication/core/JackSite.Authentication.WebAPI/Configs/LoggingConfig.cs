using Serilog;
using Serilog.Exceptions;
using System.Reflection;

namespace JackSite.Authentication.WebAPI.Configs;

public static class LoggingConfig
{
    public static WebApplicationBuilder ApplySerilog(this WebApplicationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var applicationName = assembly.GetName().Name;
        var applicationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";
        var environment = builder.Environment.EnvironmentName;
        
        // 创建启动日志器
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.WithProperty("ApplicationName", applicationName)
            .Enrich.WithProperty("Environment", environment)
            .Enrich.WithProperty("Version", applicationVersion)
            .WriteTo.Console()
            .CreateBootstrapLogger();
        
        try
        {
            Log.Information("Starting {ApplicationName} v{Version} in {Environment} environment", 
                applicationName, applicationVersion, environment);
            
            builder.Host.UseSerilog((ctx, cfg) =>
            {
                cfg.ReadFrom.Configuration(ctx.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithProperty("ApplicationName", applicationName)
                    .Enrich.WithProperty("Version", applicationVersion)
                    .Enrich.WithExceptionDetails()  // 添加详细的异常信息
                    .Destructure.ToMaximumDepth(4)  // 限制对象序列化深度
                    .Destructure.ToMaximumStringLength(100);  // 限制字符串长度
            });
            
            return builder;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            throw;
        }
    }
    
    public static void LogApplicationShutdown(this IHost host)
    {
        var applicationName = Assembly.GetExecutingAssembly().GetName().Name;
        Log.Information("{ApplicationName} is shutting down...", applicationName);
        Log.CloseAndFlush();
    }
    
    public static WebApplication UseSerilogConfig(this WebApplication app)
    {
        // 在中间件管道中使用 Serilog 请求日志记录
        app.UseSerilogRequestLogging(opts =>
        {
            opts.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                if (httpContext.Request.Host.Value != null)
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                if (httpContext.Connection.RemoteIpAddress != null)
                    diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
            };
        });

        

        app.MapHealthChecks("/health");

        // 记录应用程序启动完成
        var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion ?? "1.0.0";
        Log.Information("JackSite Authentication API v{Version} started successfully", version);
        return app;
    }
}
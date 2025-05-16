using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddApplication();

// 配置Serilog
builder.ConfigureSerilog(configuration);

builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

// 注册应用程序关闭事件
app.Lifetime.ApplicationStopping.Register(() => { Log.Information("应用程序正在关闭..."); });

app.Lifetime.ApplicationStopped.Register(() =>
{
    Log.Information("应用程序已关闭");

    // 确保所有日志都被刷新
    Log.CloseAndFlush();
});


app.UseSerilogRequestLogging(options =>
{
    // 自定义请求日志消息模板
    options.MessageTemplate = 
        "HTTP {RequestMethod} {RequestPath} 响应 {StatusCode} 用时 {Elapsed:0.0000}ms";
    
    // 自定义日志级别
    options.GetLevel = (httpContext, elapsed, ex) => 
    {
        // 根据状态码和响应时间确定日志级别
        if (ex != null)
            return LogEventLevel.Error;
        if (elapsed > 5000) // 5秒以上视为警告
            return LogEventLevel.Warning;
        return httpContext.Response.StatusCode switch
        {
            > 499 => LogEventLevel.Error,
            > 399 => LogEventLevel.Warning,
            _ => LogEventLevel.Information
        };
    };
    
    // 丰富诊断上下文
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        diagnosticContext.Set("RequestContentType", httpContext.Request.ContentType);
        diagnosticContext.Set("ResponseContentType", httpContext.Response.ContentType);
        diagnosticContext.Set("EndpointName", httpContext.GetEndpoint()?.DisplayName);
        
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            diagnosticContext.Set("UserId", httpContext.User.FindFirst("UserId")?.Value);
            diagnosticContext.Set("UserName", httpContext.User.FindFirst("UserName")?.Value);
        }
    };
});


app.UseLogContext();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "JackSite API Docs";
        opt.Layout = ScalarLayout.Modern;
    });
}

// 使用自定义的启动方法
await app.ApplicationRunAsync(CancellationToken.None);
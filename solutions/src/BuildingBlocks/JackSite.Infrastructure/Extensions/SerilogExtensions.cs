using JackSite.Infrastructure.Logging;

namespace JackSite.Infrastructure.Extensions;

public static class SerilogExtensions
{
    public static void AddDefaultSerilog(this ConfigureHostBuilder builder,
        string applicationName)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration.CreateDefaultLoggerConfiguration(
                context.Configuration,
                applicationName);
        });
    }

    public static void UseDefaultSerilogRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex != null || httpContext.Response.StatusCode > 499)
                    return LogEventLevel.Error;
                if (httpContext.Response.StatusCode > 399)
                    return LogEventLevel.Warning;
                return elapsed > 5000 ? // 5 seconds
                    LogEventLevel.Warning : LogEventLevel.Information;
            };

            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
            };
        });
    }
}
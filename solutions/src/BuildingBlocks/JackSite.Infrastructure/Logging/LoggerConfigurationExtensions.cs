namespace JackSite.Infrastructure.Logging;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration CreateDefaultLoggerConfiguration(
        this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration,
        string applicationName)
    {
        return loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", applicationName)
            .Enrich.WithMachineName()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code)
            .WriteTo.Seq(
                serverUrl: configuration["Seq:ServerUrl"] ?? "http://localhost:5341",
                apiKey: configuration["Seq:ApiKey"]);
    }
}
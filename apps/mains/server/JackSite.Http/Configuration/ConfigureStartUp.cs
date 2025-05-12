using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Reflection;

namespace JackSite.Http.Configuration;

public static class ConfigureStartUp
{
    public static async Task ApplicationRunAsync(this WebApplication app, CancellationToken cancellationToken)
    {
        // 使用静态 Log 类而不是 ILogService
        var env = app.Environment;
        var appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "JackSite.Http";
        var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0";

        // 记录应用程序启动信息
        Log.Information(
            "应用程序 {ApplicationName} v{Version} 正在启动 - 环境: {Environment}",
            appName,
            version,
            env.EnvironmentName);

        // 记录主机信息
        Log.Information(
            "主机: {MachineName} ({OSVersion}), 进程ID: {ProcessId}",
            Environment.MachineName,
            Environment.OSVersion,
            Environment.ProcessId);

        // 记录.NET运行时信息
        Log.Information(
            ".NET运行时: {FrameworkDescription}, CPU: {ProcessorCount} 核心",
            Environment.Version,
            Environment.ProcessorCount);

        // 记录配置信息
        var config = app.Configuration;
        Log.Information(
            "配置源: {ConfigurationSources}",
            string.Join(", ", ((IConfigurationRoot)config).Providers.Select(p => p.GetType().Name)));

        // 记录应用程序URL配置
        var urls = app.Configuration["urls"] ?? app.Configuration["ASPNETCORE_URLS"];
        if (!string.IsNullOrEmpty(urls))
        {
            Log.Information("配置的URL: {Urls}/scalar/v1", urls);
        }

        // 启动应用程序
        await app.RunAsync(cancellationToken);

        // 获取服务器地址
        try
        {
            var server = app.Services.GetService<IServer>();
            var addresses = server?.Features.Get<IServerAddressesFeature>()?.Addresses;

            if (addresses != null && addresses.Count != 0)
            {
                Log.Information(
                    "应用程序已启动，正在监听: {Addresses}",
                    string.Join(", ", addresses));

                // 为每个地址单独记录详细信息
                foreach (var address in addresses)
                {
                    try
                    {
                        var uri = new Uri(address);
                        Log.Information(
                            "监听端点: {Scheme}://{Host}:{Port}",
                            uri.Scheme,
                            uri.Host,
                            uri.Port);
                    }
                    catch
                    {
                        // 如果URI解析失败，只记录原始地址
                        Log.Information("监听端点: {Address}", address);
                    }
                }
            }
            else
            {
                Log.Information("应用程序已启动，但未找到监听地址");
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "无法获取服务器地址");
            Log.Information("应用程序已启动");
        }
    }
}
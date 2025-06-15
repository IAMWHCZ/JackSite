using System.Text.RegularExpressions;
using JackSite.Authentication.WebAPI.Modules;

namespace JackSite.Authentication.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapApiModules(this WebApplication app)
    {
        // 获取所有注册的模块并应用路由
        var modules = app.Services.GetServices<IApiModule>();
        foreach (var module in modules)
        {
            var baseUrl = GetBaseUrlFromClassName(module.GetType().Name);
            
            var group = app.MapGroup($"/api/{baseUrl}")
                .WithTags(baseUrl)
                .WithName(baseUrl)
                .WithOpenApi()
                .WithSummary($"API for {baseUrl}");
            
            module.AddRoutes(group);
            
        }

        return app;
    }
    private static string GetBaseUrlFromClassName(string className)
    {
        foreach (var suffix in new[] { "Endpoint", "Module", "Controller" })
        {
            if (className.EndsWith(suffix,StringComparison.OrdinalIgnoreCase))
            {
                className = className[..^suffix.Length];
                break;
            }
        }

        var path = Regex.Replace(className, "([a-z])([A-Z])", "$1-$2").ToLower();
        return path;
    }
}
using JackSite.Http.Modules;

namespace JackSite.Http.Configuration;

public static class ConfigureApplication
{
    public static void RegisterModules(this WebApplication app, Assembly assembly)
    {
        app.RegisterApiModules(assembly);

        app.UseMiddleware<HeaderParamsMiddleware>();
    }
}
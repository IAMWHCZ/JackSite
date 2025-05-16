namespace JackSite.Http.Configuration;

public static class ConfigureApplication
{
    public static void RegisterModules(this WebApplication app, Assembly assembly)
    {
        // 查找所有继承自 Module 的类
        var moduleTypes = assembly.GetTypes()
            .Where(t => typeof(EndpointModule).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var moduleType in moduleTypes)
        {
            if (Activator.CreateInstance(moduleType) is EndpointModule module)
            {
                var routeGroup = app.MapGroup("api");
                module.AddRoutes(routeGroup);
            }
        }
    }

}
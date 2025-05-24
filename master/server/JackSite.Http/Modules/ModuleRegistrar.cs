namespace JackSite.Http.Modules;

/// <summary>
/// 模块注册器，用于自动发现和注册API模块
/// </summary>
public static class ModuleRegistrar
{
    /// <summary>
    /// 注册所有API模块
    /// </summary>
    public static WebApplication RegisterApiModules(this WebApplication app, Assembly assembly)
    {
        // 查找所有实现IApiModule的非抽象类
        var moduleTypes = assembly.GetTypes()
            .Where(t => typeof(IApiModule).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var moduleType in moduleTypes)
        {
            if (Activator.CreateInstance(moduleType) is IApiModule module)
            {
                // 创建模块路由组
                var routeGroup = app.MapGroup("/");
                
                module.AddRoutes(routeGroup);
                
                Log.Debug("已注册API模块: {ModuleName}", moduleType.Name);
            }
        }
        
        return app;
    }
}
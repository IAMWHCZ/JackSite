namespace JackSite.Shared.Http.Abstractions;

/// <summary>
/// 最小化 API 端点基类
/// </summary>
public abstract class MinimalApiEndpointBase : IMinimalApiEndpoint
{
    /// <summary>
    /// 获取 API 路由前缀
    /// </summary>
    protected abstract string RoutePrefix { get; }

    /// <summary>
    /// 映射端点
    /// </summary>
    /// <param name="app">Web 应用程序</param>
    public virtual void MapEndpoints(WebApplication app)
    {
        var group = app.MapGroup(RoutePrefix);
        ConfigureEndpoints(group);
    }

    /// <summary>
    /// 映射端点到路由组
    /// </summary>
    /// <param name="group">路由组构建器</param>
    public virtual void MapEndpoints(RouteGroupBuilder group)
    {
        var subGroup = group.MapGroup(RoutePrefix);
        ConfigureEndpoints(subGroup);
    }

    /// <summary>
    /// 配置端点
    /// </summary>
    /// <param name="group">路由组构建器</param>
    protected abstract void ConfigureEndpoints(RouteGroupBuilder group);
}
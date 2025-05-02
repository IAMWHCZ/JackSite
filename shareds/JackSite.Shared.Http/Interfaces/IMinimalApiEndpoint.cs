namespace JackSite.Shared.Http.Interfaces;

/// <summary>
/// 最小化 API 端点接口
/// </summary>
public interface IMinimalApiEndpoint
{
    /// <summary>
    /// 映射端点
    /// </summary>
    /// <param name="app">Web 应用程序</param>
    void MapEndpoints(WebApplication app);
    
    /// <summary>
    /// 映射端点到路由组
    /// </summary>
    /// <param name="group">路由组构建器</param>
    void MapEndpoints(RouteGroupBuilder group);
}

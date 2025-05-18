namespace JackSite.Http.Modules;

/// <summary>
/// API模块接口，用于定义模块化API注册
/// </summary>
public interface IApiModule
{
    /// <summary>
    /// 添加路由到应用程序
    /// </summary>
    void AddRoutes(IEndpointRouteBuilder endpoints);
}
using MediatR;

namespace JackSite.Http.Modules;

public abstract class EndpointModule : IApiModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder endpoints);
    
    protected T GetService<T>(HttpContext context) => context.RequestServices.GetRequiredService<T>();
    
    /// <summary>
    /// 获取MediatR发送器
    /// </summary>
    protected ISender GetSender(HttpContext context) => GetService<ISender>(context);
}

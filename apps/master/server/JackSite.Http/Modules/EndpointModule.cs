namespace JackSite.Http.Modules;

public abstract class EndpointModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder endpoints);
    
    protected T GetService<T>(HttpContext context) => context.RequestServices.GetRequiredService<T>();
}
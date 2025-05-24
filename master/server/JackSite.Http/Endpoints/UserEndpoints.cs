using JackSite.Http.Modules;

namespace JackSite.Http.Endpoints;

public class UserEndpoints:EndpointModule
{
    public override void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users").WithTags("Users");

        endpoints.MapGet("/api/users", async () =>
        {
            return "test";
        });
    }
}
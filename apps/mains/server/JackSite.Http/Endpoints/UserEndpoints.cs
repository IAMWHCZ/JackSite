using JackSite.Http.Modules;
using MediatR;

namespace JackSite.Http.Endpoints;

public class UserEndpoints(ISender sender):EndpointModule
{
    public override void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users", async () =>
        {
            
        });
    }
}
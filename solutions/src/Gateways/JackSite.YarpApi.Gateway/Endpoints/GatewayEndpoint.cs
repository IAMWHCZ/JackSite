using Carter;
using JackSite.YarpApi.Gateway.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JackSite.YarpApi.Gateway.Endpoints;

public class GatewayEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/gateway");

        group.MapPost("access-records", async ([FromBody] GetGatewayRequestQuery request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        })
        .WithName("GetAccessRecords");
    }
}
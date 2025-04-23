using Carter;
using JackSite.Common.Results;
using JackSite.YarpApi.Gateway.Commands;
using JackSite.YarpApi.Gateway.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JackSite.YarpApi.Gateway.Endpoints;

public class GatewayConfigEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/gateway-config");

        group.MapGet("list/{type:int}", async (
            [FromRoute] int type,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            dynamic result = type == 1
                ? await sender.Send(new GetGatewayRouteConfigQuery(), cancellationToken)
                : await sender.Send(new GetGatewayClusterConfigQuery(), cancellationToken);
            return Results.Ok(result);
        })
            .WithName("获取网关配置")
            .WithDescription("获取网关配置")
            .ProducesProblem(400);

        group.MapPost("route", async (
            [FromBody] CreateRouteCommand command,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        })
            .WithName("添加路由配置")
            .WithDescription("添加路由配置")
            .ProducesProblem(400);

        group.MapPut("route", async (
            [FromBody] UpdateRouteCommand command,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        })
            .WithName("更新路由配置")
            .WithDescription("更新路由配置")
            .ProducesProblem(400);

        group.MapDelete("route", async (
            [FromBody] DeleteRouteCommand command,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        })
            .WithName("删除路由配置")
            .WithDescription("删除路由配置")
            .ProducesProblem(400);

        group.MapPost("cluster", async (
            [FromBody] CreateClusterCommand command,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        })
            .WithName("添加集群配置")
            .WithDescription("添加集群配置")
            .ProducesProblem(400);

        group.MapPut("cluster", async (
            [FromBody] UpdateClusterCommand command,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        })
            .WithName("更新集群配置")
            .WithDescription("更新集群配置")
            .ProducesProblem(400);

        group.MapDelete("cluster", async (
            [FromBody] DeleteClusterCommand command,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        })
            .WithName("删除集群配置")
            .WithDescription("删除集群配置")
            .ProducesProblem(400);

        group.MapGet("reload/{type:int}", async (
            [FromRoute] int type,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new ReloadConfigCommand(type), cancellationToken);
            return Results.Ok(result);
        })
            .WithName("重新加载配置")
            .WithDescription("重新加载配置")
            .ProducesProblem(400);
    }
}


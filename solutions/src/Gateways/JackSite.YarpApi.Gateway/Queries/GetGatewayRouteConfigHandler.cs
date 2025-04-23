using JackSite.Common.CQRS;
using JackSite.Common.Results;

using Yarp.ReverseProxy.Configuration;

namespace JackSite.YarpApi.Gateway.Queries;

public record GetGatewayRouteConfigQuery : IQuery<Result<GetGatewayRouteConfigResult>>;
public record GetGatewayRouteConfigResult(IReadOnlyList<RouteConfig> Routes);

internal sealed class GetGatewayRouteConfigHandler(IProxyConfigProvider proxyConfigProvider)
    : IQueryHandler<GetGatewayRouteConfigQuery,
        Result<GetGatewayRouteConfigResult>>
{
    public Task<Result<GetGatewayRouteConfigResult>> Handle(
        GetGatewayRouteConfigQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var config = proxyConfigProvider.GetConfig();
            var routes = config.Routes; 
            var result = new GetGatewayRouteConfigResult(routes);
            return Task.FromResult(Result.Success(result));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result.Failure<GetGatewayRouteConfigResult>(
                $"Failed to read YARP configuration: {ex.Message}"));
        }
    }
}
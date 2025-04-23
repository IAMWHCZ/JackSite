using JackSite.Common.CQRS;
using JackSite.Common.Results;
using Yarp.ReverseProxy.Configuration;

namespace JackSite.YarpApi.Gateway.Queries;

public record GetGatewayClusterConfigQuery : IQuery<Result<GetGatewayClusterConfigResult>>;

public record GetGatewayClusterConfigResult(IReadOnlyList<ClusterConfig> Cluster);

internal sealed class GetGatewayClusterHandler(IProxyConfigProvider proxyConfigProvider)
    : IQueryHandler<GetGatewayClusterConfigQuery,
        Result<GetGatewayClusterConfigResult>>
{
    public Task<Result<GetGatewayClusterConfigResult>> Handle(GetGatewayClusterConfigQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var config = proxyConfigProvider.GetConfig();
            var cluster = config.Clusters;
            var result = new GetGatewayClusterConfigResult(cluster);
            return Task.FromResult(Result.Success(result));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result.Failure<GetGatewayClusterConfigResult>(
                $"Failed to read YARP configuration: {ex.Message}"));
        }
    }
}
using System.Text.Json;
using JackSite.Common.CQRS;
using JackSite.Common.Results;
using JackSite.YarpApi.Gateway.Const;
using JackSite.YarpApi.Gateway.Data;
using JackSite.YarpApi.Gateway.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JackSite.YarpApi.Gateway.Commands;

public record ReloadConfigCommand(int Type) : ICommand<Result<IReadOnlyList<object>>>;

internal sealed class ReloadConfigHandler(
    GatewayDbContext dbContext,
    IConfiguration configuration
    ) : IRequestHandler<ReloadConfigCommand, Result<IReadOnlyList<object>>>
{
    public async Task<Result<IReadOnlyList<object>>> Handle(ReloadConfigCommand request, CancellationToken cancellationToken)
    {
        var configPath = SystemConst.ConfigPath;
        var jsonString = await File.ReadAllTextAsync(configPath, cancellationToken);
        var jsonDoc = JsonDocument.Parse(jsonString);
        var root = jsonDoc.RootElement.Clone();
        var configObject = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(root.GetRawText());
        if (configObject == null) return Result.Failure<IReadOnlyList<object>>("Failed to parse configuration file");
        if (!configObject.TryGetValue("ReverseProxy", out var value))
        {
            value = JsonSerializer.SerializeToElement(new
            {
                Routes = new Dictionary<string, object>(),
                Clusters = new Dictionary<string, object>()
            });
            configObject["ReverseProxy"] = value;
        }
        var reverseProxy = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(value.GetRawText());
        if (reverseProxy == null) return Result.Failure<IReadOnlyList<object>>("Invalid ReverseProxy configuration");
        var config =
            await dbContext.YarpConfigs.FirstOrDefaultAsync(x => x.Id == DatabaseConst.SnowflakeId, cancellationToken);
        if (config == null) return Result.Failure<IReadOnlyList<object>>("Failed to read YARP configuration");
        var originDoc = JsonDocument.Parse(config.ConfigJson);
        var originRoot = originDoc.RootElement;

        
        var routes = originRoot.GetProperty("Routes")
    .EnumerateArray()
    .Where(route => route.GetProperty("RouteId").GetString() != null)
    .ToDictionary(
        route => route.GetProperty("RouteId").GetString()!,  // 使用 null 断言操作符
        route => JsonSerializer.Deserialize<RouteConfig>(route.GetRawText())
    );

        var clusters = originRoot.GetProperty("Clusters")
            .EnumerateArray()
            .Where(cluster => cluster.GetProperty("ClusterId").GetString() != null)
            .ToDictionary(
                cluster => cluster.GetProperty("ClusterId").GetString()!,  // 使用 null 断言操作符
                cluster => JsonSerializer.Deserialize<ClusterConfig>(cluster.GetRawText())
            );

        var yarpConfig = new YarpConfig
        {
            Routes = routes!,
            Clusters = clusters!
        };

        if (yarpConfig == null) return Result.Failure<IReadOnlyList<object>>("Failed to read YARP configuration");
        var result =
            JsonSerializer.Deserialize<(IReadOnlyList<ClusterConfig>, IReadOnlyList<RouteConfig>)>(
                config.ConfigJson);
        switch (request.Type)
        {
            case 1:
                reverseProxy["Routes"] = JsonSerializer.SerializeToElement(yarpConfig.Routes);
                configObject["ReverseProxy"] = JsonSerializer.SerializeToElement(reverseProxy);
                var updatedRouteJson = JsonSerializer.Serialize(configObject, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(configPath, updatedRouteJson, cancellationToken);
                return Result.Success<IReadOnlyList<object>>(result.Item2);
            case 2:
                reverseProxy["Clusters"] = JsonSerializer.SerializeToElement(yarpConfig.Clusters);
                configObject["ReverseProxy"] = JsonSerializer.SerializeToElement(reverseProxy);
                var updatedClusterJson = JsonSerializer.Serialize(configObject, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(configPath, updatedClusterJson, cancellationToken);
                return Result.Success<IReadOnlyList<object>>(result.Item1);
            case 3:
                reverseProxy["Routes"] = JsonSerializer.SerializeToElement(yarpConfig.Routes);
                reverseProxy["Clusters"] = JsonSerializer.SerializeToElement(yarpConfig.Clusters);
                configObject["ReverseProxy"] = JsonSerializer.SerializeToElement(reverseProxy);
                var updatedJson = JsonSerializer.Serialize(configObject, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(configPath, updatedJson, cancellationToken);
                return Result.Success<IReadOnlyList<object>>([]);
        }
        if (configuration is IConfigurationRoot configRoot) configRoot.Reload();
        
        return Result.Failure<IReadOnlyList<object>>("Invalid type");
    }
}

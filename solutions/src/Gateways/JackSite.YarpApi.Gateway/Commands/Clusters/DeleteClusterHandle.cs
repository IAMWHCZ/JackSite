using System.Text.Json;
using JackSite.Common.CQRS;
using JackSite.Common.Results;
using MediatR;
using Yarp.ReverseProxy.Configuration;

namespace JackSite.YarpApi.Gateway.Commands;

public record DeleteClusterCommand(string ClusterId) : ICommand<Result<Unit>>;

internal sealed class DeleteClusterHandler(IConfiguration configuration)
    : ICommandHandler<DeleteClusterCommand, Result<Unit>>
{
    private const string ConfigPath = "appsettings.json";

    public async Task<Result<Unit>> Handle(DeleteClusterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 读取现有配置
            var jsonString = await File.ReadAllTextAsync(ConfigPath, cancellationToken);
            var jsonDoc = JsonDocument.Parse(jsonString);
            var root = jsonDoc.RootElement.Clone();
            var configObject = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(root.GetRawText());

            if (configObject == null)
            {
                return Result.Failure<Unit>("Failed to parse configuration file");
            }

            // 获取ReverseProxy配置
            if (!configObject.TryGetValue("ReverseProxy", out var value))
            {
                return Result.Failure<Unit>("ReverseProxy configuration not found");
            }

            var reverseProxy = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
                value.GetRawText());

            if (reverseProxy == null)
            {
                return Result.Failure<Unit>("Invalid ReverseProxy configuration");
            }

            // 获取集群配置
            var clusters = JsonSerializer.Deserialize<Dictionary<string, object>>(
                reverseProxy["Clusters"].GetRawText()) ?? new Dictionary<string, object>();

            // 检查集群是否存在
            if (!clusters.ContainsKey(request.ClusterId))
            {
                return Result.Failure<Unit>($"Cluster with ID '{request.ClusterId}' not found");
            }

            // 检查是否有路由引用此集群
            var routes = JsonSerializer.Deserialize<Dictionary<string, RouteConfig>>(
                reverseProxy["Routes"].GetRawText()) ?? new Dictionary<string, RouteConfig>();

            if (routes.Any(r => JsonSerializer.Deserialize<RouteConfig>(
                JsonSerializer.Serialize(r.Value))?.ClusterId == request.ClusterId))
            {
                return Result.Failure<Unit>(
                    $"Cannot delete cluster '{request.ClusterId}' because it is referenced by one or more routes");
            }

            // 删除集群
            clusters.Remove(request.ClusterId);

            // 更新配置文件
            reverseProxy["Clusters"] = JsonSerializer.SerializeToElement(clusters);
            configObject["ReverseProxy"] = JsonSerializer.SerializeToElement(reverseProxy);

            var updatedJson = JsonSerializer.Serialize(configObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(ConfigPath, updatedJson, cancellationToken);

            // 重新加载配置
            if (configuration is IConfigurationRoot configRoot)
            {
                configRoot.Reload();
            }

            return Result.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<Unit>($"Failed to delete cluster configuration: {ex.Message}");
        }
    }
}
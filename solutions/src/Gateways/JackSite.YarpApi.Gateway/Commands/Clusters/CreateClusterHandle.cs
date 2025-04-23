using System.Text.Json;
using JackSite.Common.CQRS;
using JackSite.Common.Results;
using MediatR;
using Yarp.ReverseProxy.Configuration;

namespace JackSite.YarpApi.Gateway.Commands;

public record CreateClusterCommand(string ClusterId, ClusterConfig ClusterConfig) : ICommand<Result<Unit>>;

internal sealed class CreateClusterHandler(IConfiguration configuration)
    : ICommandHandler<CreateClusterCommand, Result<Unit>>
{
    private const string ConfigPath = "appsettings.json";

    public async Task<Result<Unit>> Handle(CreateClusterCommand request, CancellationToken cancellationToken)
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

            // 获取或创建 ReverseProxy 部分
            if (!configObject.TryGetValue("ReverseProxy", out var value))
            {
                value = JsonSerializer.SerializeToElement(new
                {
                    Routes = new Dictionary<string, object>(),
                    Clusters = new Dictionary<string, object>()
                });
                configObject["ReverseProxy"] = value;
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

            // 检查集群ID是否已存在
            if (clusters.ContainsKey(request.ClusterId))
            {
                return Result.Failure<Unit>($"Cluster with ID '{request.ClusterId}' already exists");
            }

            // 添加新集群
            clusters[request.ClusterId] = request.ClusterConfig;

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
            return Result.Failure<Unit>($"Failed to create cluster configuration: {ex.Message}");
        }
    }
}
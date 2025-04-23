using System.Text.Json;
using JackSite.Common.CQRS;
using JackSite.Common.Results;
using MediatR;
using Yarp.ReverseProxy.Configuration;

namespace JackSite.YarpApi.Gateway.Commands;

public record CreateRouteCommand(string RouteId, RouteConfig RouteConfig) : ICommand<Result<Unit>>;

internal sealed class CreateRouteHandler(IConfiguration configuration)
    : ICommandHandler<CreateRouteCommand, Result<Unit>>
{
    private const string ConfigPath = "appsettings.json";

    public async Task<Result<Unit>> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
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

            // 获取路由配置
            var routes = JsonSerializer.Deserialize<Dictionary<string, object>>(
                reverseProxy["Routes"].GetRawText()) ?? new Dictionary<string, object>();

            // 检查路由ID是否已存在
            if (routes.ContainsKey(request.RouteId))
            {
                return Result.Failure<Unit>($"Route with ID '{request.RouteId}' already exists");
            }

            // 添加新路由
            routes[request.RouteId] = request.RouteConfig;

            // 更新配置文件
            reverseProxy["Routes"] = JsonSerializer.SerializeToElement(routes);
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
            return Result.Failure<Unit>($"Failed to create route configuration: {ex.Message}");
        }
    }
}
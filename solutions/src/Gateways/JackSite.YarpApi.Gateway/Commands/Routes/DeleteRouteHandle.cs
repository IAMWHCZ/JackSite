using System.Text.Json;
using JackSite.Common.CQRS;
using JackSite.Common.Results;
using MediatR;

namespace JackSite.YarpApi.Gateway.Commands;

public record DeleteRouteCommand(string RouteId) : ICommand<Result<Unit>>;

internal sealed class DeleteRouteHandler(IConfiguration configuration)
    : ICommandHandler<DeleteRouteCommand, Result<Unit>>
{
    private const string ConfigPath = "appsettings.json";

    public async Task<Result<Unit>> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
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

            // 获取路由配置
            var routes = JsonSerializer.Deserialize<Dictionary<string, object>>(
                reverseProxy["Routes"].GetRawText()) ?? new Dictionary<string, object>();

            // 检查路由是否存在
            if (!routes.ContainsKey(request.RouteId))
            {
                return Result.Failure<Unit>($"Route with ID '{request.RouteId}' not found");
            }

            // 删除路由
            routes.Remove(request.RouteId);

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
            return Result.Failure<Unit>($"Failed to delete route configuration: {ex.Message}");
        }
    }
}
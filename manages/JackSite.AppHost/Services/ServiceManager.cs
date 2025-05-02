using JackSite.AppHost.Interfaces;

namespace JackSite.AppHost.Services;

public class ServiceManager(
    IDistributedApplicationBuilder builder,
    IConfiguration config,
    Dictionary<string, object> resources)
{
    private readonly List<IServiceConfigurator> _configurators =
    [
        new IdentityServiceConfigurator(),
        new BlogServiceConfigurator(),
        new ToolsServiceConfigurator(),
        new ApiGatewayConfigurator(),
        new UserServiceConfigurator()
    ];
    private readonly Dictionary<string, IResourceBuilder<IResource>> _services = new();

    // 初始化所有服务配置器

    public void ConfigureServices()
    {
        // 配置所有服务
        foreach (var configurator in _configurators.Where(c => c.ServiceName != "api-gateway"))
        {
            var service = configurator.Configure(builder, config, resources);
            _services[configurator.ServiceName] = service;
        }
            
        // 配置API网关（需要依赖其他服务）
        var apiGatewayConfigurator = _configurators.First(c => c.ServiceName == "api-gateway");
        var apiGateway = apiGatewayConfigurator.Configure(builder, config, resources);
            
        // 添加对其他服务的引用
        // 注意：这里我们需要确保 apiGateway 实现了 IResourceWithEnvironment 接口
        if (apiGateway is IResourceBuilder<IResourceWithEnvironment> apiGatewayWithEnv)
        {
            foreach (var service in _services.Values)
            {
                if (service is IResourceBuilder<IResourceWithServiceDiscovery> serviceWithDiscovery)
                {
                    // 使用正确的重载方法
                    apiGatewayWithEnv.WithReference(serviceWithDiscovery);
                }
            }
        }
    }
}
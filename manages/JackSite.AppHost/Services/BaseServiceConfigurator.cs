using JackSite.AppHost.Interfaces;

namespace JackSite.AppHost.Services;

public abstract class BaseServiceConfigurator : IServiceConfigurator
{
    public abstract string ServiceName { get; }
        
    public abstract IResourceBuilder<IResource> Configure(IDistributedApplicationBuilder builder, IConfiguration config, Dictionary<string, object> resources);

    // 辅助方法：获取必需的配置值
    protected string GetRequiredConfig(IConfiguration config, string key)
    {
        var value = config[key];
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"配置项 '{key}' 不能为空");
        }
        return value;
    }

    // 辅助方法：添加服务引用
    protected static void AddServiceReferences<T>(IResourceBuilder<T> service, string[] resourceNames, Dictionary<string, object> resources) 
        where T : IResource, IResourceWithEnvironment
    {
        foreach (var name in resourceNames)
        {
            if (resources.TryGetValue(name, out var resource) && 
                resource is IResourceBuilder<IResourceWithServiceDiscovery> resourceWithDiscovery)
            {
                service.WithReference(resourceWithDiscovery);
            }
        }
    }

    // 辅助方法：添加服务关系
    protected static void AddServiceRelationships<T>(IResourceBuilder<T> service, string[] resourceNames, Dictionary<string, object> resources) 
        where T : IResource
    {
        foreach (var name in resourceNames)
        {
            if (resources.TryGetValue(name, out var resource) && 
                resource is IResourceBuilder<IResource> resourceBuilder)
            {
                // 注意：这里可能需要根据实际情况调整
                // WithReferenceRelationship 方法可能需要特定类型的参数
                // 如果没有合适的方法，可能需要自定义实现
                // 暂时注释掉，避免编译错误
                // service.WithReferenceRelationship(resourceBuilder);
            }
        }
    }
}
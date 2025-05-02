using JackSite.AppHost.Interfaces;

namespace JackSite.AppHost.Infrastructure;

public abstract class BaseComponentConfigurator : IComponentConfigurator
{
    protected object _resource = null!;
        
    public abstract void Configure(IDistributedApplicationBuilder builder, IConfiguration config);
    public abstract string ResourceName { get; }
    public object Resource => _resource;

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

    // 辅助方法：获取必需的整数配置值
    protected int GetRequiredIntConfig(IConfiguration config, string key)
    {
        var value = GetRequiredConfig(config, key);
        if (!int.TryParse(value, out var intValue))
        {
            throw new InvalidOperationException($"配置项 '{key}' 必须是有效的整数");
        }
        return intValue;
    }

    // 辅助方法：添加端点
    protected void AddEndpoint<T>(IResourceBuilder<T> resource, IConfiguration section, string portKey, string endpointName) 
        where T : IResource, IResourceWithEndpoints
    {
        var port = GetRequiredIntConfig(section, portKey);
        resource.WithEndpoint(port, port, name: endpointName);
    }

    // 辅助方法：添加环境变量
    protected void AddEnvironmentVariables<T>(IResourceBuilder<T> resource, IConfiguration section, (string EnvName, string ConfigKey)[] variables) 
        where T : IResource, IResourceWithEnvironment
    {
        foreach (var (envName, configKey) in variables)
        {
            var value = GetRequiredConfig(section, configKey);
            resource.WithEnvironment(envName, value);
        }
    }
}
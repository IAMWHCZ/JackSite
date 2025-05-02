var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject("servicedefaults", "../JackSite.ServiceDefaults/JackSite.ServiceDefaults.csproj");

// 获取配置
var configuration = builder.Configuration;
var infrastructureConfig = configuration.GetSection("Infrastructure");
var servicesConfig = configuration.GetSection("Services");

// 验证配置是否存在
if (!infrastructureConfig.Exists())
{
    throw new InvalidOperationException("未找到基础设施配置节点 'Infrastructure'");
}

if (!servicesConfig.Exists())
{
    throw new InvalidOperationException("未找到服务配置节点 'Services'");
}

// 配置基础设施
var infrastructureManager = new InfrastructureManager(builder, infrastructureConfig);
var resources = infrastructureManager.ConfigureInfrastructure();

// 配置服务
var serviceManager = new ServiceManager(builder, servicesConfig, resources);
serviceManager.ConfigureServices();

await builder.Build().RunAsync();

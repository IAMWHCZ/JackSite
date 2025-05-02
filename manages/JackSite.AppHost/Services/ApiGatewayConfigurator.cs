namespace JackSite.AppHost.Services;

public class ApiGatewayConfigurator : BaseServiceConfigurator
{
    public override string ServiceName => "api-gateway";

    public override IResourceBuilder<IResource> Configure(IDistributedApplicationBuilder builder, IConfiguration config, Dictionary<string, object> resources)
    {
        var section = config.GetSection("ApiGateway");
        var apiGateway = builder.AddProject(ServiceName, GetRequiredConfig(section, "ProjectPath"));
            
        // 添加服务引用
        // 注意：这里我们需要确保 apiGateway 实现了 IResourceWithEnvironment 接口
        if (apiGateway is IResourceBuilder<IResourceWithEnvironment> apiGatewayWithEnv)
        {
            if (resources.TryGetValue("seq", out var seq) && 
                seq is IResourceBuilder<IResourceWithServiceDiscovery> seqResource)
            {
                apiGatewayWithEnv.WithReference(seqResource);
            }
        }
            
        return apiGateway;
    }
}
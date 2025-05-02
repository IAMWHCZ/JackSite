namespace JackSite.AppHost.Services;

public class IdentityServiceConfigurator : BaseServiceConfigurator
{
    public override string ServiceName => "identity";

    public override IResourceBuilder<IResource> Configure(IDistributedApplicationBuilder builder, IConfiguration config, Dictionary<string, object> resources)
    {
        var section = config.GetSection("Identity");
        var identityService = builder.AddProject(ServiceName, GetRequiredConfig(section, "ProjectPath"));
            
        // 添加服务引用
        AddServiceReferences(identityService, ["postgres", "redis", "seq"], resources);
            
        return identityService;
    }
}
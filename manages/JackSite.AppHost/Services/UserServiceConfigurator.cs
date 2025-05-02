namespace JackSite.AppHost.Services;

public class UserServiceConfigurator:BaseServiceConfigurator
{
    public override string ServiceName => "user";

    public override IResourceBuilder<IResource> Configure(IDistributedApplicationBuilder builder, IConfiguration config, Dictionary<string, object> resources)
    {
        var section = config.GetSection("User");
        var toolsService = builder.AddProject(ServiceName, GetRequiredConfig(section, "ProjectPath"));
            
        // 添加服务引用
        AddServiceReferences(toolsService, ["postgres", "redis", "seq"], resources);
            
        // 添加服务关系
        AddServiceRelationships(toolsService, ["minio"], resources);
            
        return toolsService;
    }
}
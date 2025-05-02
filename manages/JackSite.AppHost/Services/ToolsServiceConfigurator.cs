namespace JackSite.AppHost.Services;

public class ToolsServiceConfigurator : BaseServiceConfigurator
{
    public override string ServiceName => "tools";

    public override IResourceBuilder<IResource> Configure(IDistributedApplicationBuilder builder, IConfiguration config, Dictionary<string, object> resources)
    {
        var section = config.GetSection("Tools");
        var toolsService = builder.AddProject(ServiceName, GetRequiredConfig(section, "ProjectPath"));
            
        // 添加服务引用
        AddServiceReferences(toolsService, ["postgres", "redis", "seq"], resources);
            
        // 添加服务关系
        AddServiceRelationships(toolsService, ["minio"], resources);
            
        return toolsService;
    }
}
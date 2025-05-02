namespace JackSite.AppHost.Services;

public class BlogServiceConfigurator : BaseServiceConfigurator
{
    public override string ServiceName => "blog";

    public override IResourceBuilder<IResource> Configure(IDistributedApplicationBuilder builder, IConfiguration config, Dictionary<string, object> resources)
    {
        var section = config.GetSection("Blog");
        var blogService = builder.AddProject(ServiceName, GetRequiredConfig(section, "ProjectPath"));
            
        // 添加服务引用
        AddServiceReferences(blogService, ["postgres", "redis", "seq", "mongodb"], resources);
            
        // 添加服务关系
        AddServiceRelationships(blogService, ["elasticsearch", "minio"], resources);
            
        return blogService;
    }
}
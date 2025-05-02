namespace JackSite.AppHost.Plugins;

public class ElasticsearchConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "elasticsearch";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("Elasticsearch");
        var elasticsearch = builder.AddContainer(ResourceName, GetRequiredConfig(section, "ImageName"));
            
        // 配置卷和环境变量
        elasticsearch.WithVolume("es-data", "/usr/share/elasticsearch/data")
            .WithEnvironment("discovery.type", "single-node")
            .WithEnvironment("xpack.security.enabled", "false");
            
        // 添加端点
        AddEndpoint(elasticsearch, section, "Port", "elasticsearch-port");
            
        _resource = elasticsearch;
    }
}
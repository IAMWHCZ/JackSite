namespace JackSite.AppHost.Plugins;

public class SeqConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "seq";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("Seq");
        var seq = builder.AddSeq(ResourceName);
            
        // 配置镜像
        seq.WithImage(GetRequiredConfig(section, "ImageName"));
            
        // 添加端点
        seq.WithEndpoint(GetRequiredIntConfig(section, "Port"), 80, name: "seq-ui")
            .WithEndpoint(GetRequiredIntConfig(section, "IngestPort"), 5341, name: "ingest");
            
        _resource = seq;
    }
}
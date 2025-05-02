namespace JackSite.AppHost.Plugins;

public class PrometheusConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "prometheus";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("Prometheus");
        var prometheus = builder.AddContainer(ResourceName, GetRequiredConfig(section, "ImageName"));
            
        // 配置卷
        prometheus.WithVolume("prometheus-data", "/prometheus");
            
        // 添加端点
        AddEndpoint(prometheus, section, "Port", "prometheus-port");
            
        _resource = prometheus;
    }
}
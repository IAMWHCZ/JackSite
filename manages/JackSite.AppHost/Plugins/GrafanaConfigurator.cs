using JackSite.AppHost.Infrastructure;
using Microsoft.Extensions.Configuration;
using Aspire.Hosting.ApplicationModel;

namespace JackSite.AppHost.Plugins;

public class GrafanaConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "grafana";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        // 直接使用传入的配置对象，它应该已经指向了 Infrastructure 节点
        var section = config.GetSection("Grafana");
        
        var grafana = builder.AddContainer(ResourceName, GetRequiredConfig(section, "ImageName"));
            
        // 配置卷
        grafana.WithVolume("grafana-data", "/var/lib/grafana");
            
        // 添加环境变量
        grafana.WithEnvironment("GF_SECURITY_ADMIN_USER", GetRequiredConfig(section, "User"));
        grafana.WithEnvironment("GF_SECURITY_ADMIN_PASSWORD", GetRequiredConfig(section, "Password"));
            
        // 添加端点
        AddEndpoint(grafana, section, "Port", "grafana-port");
            
        _resource = grafana;
    }
}
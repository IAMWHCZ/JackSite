namespace JackSite.AppHost.Plugins;

public class RabbitMqConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "rabbitmq";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("RabbitMQ");
        var rabbitmq = builder.AddRabbitMQ(ResourceName);
            
        // 配置镜像
        rabbitmq.WithImage(GetRequiredConfig(section, "ImageName"));
            
        // 添加端点
        AddEndpoint(rabbitmq, section, "Port", "rabbitmq-port");
        AddEndpoint(rabbitmq, section, "ManagementPort", "management");
            
        // 添加环境变量
        AddEnvironmentVariables(rabbitmq, section, [
            ("RABBITMQ_DEFAULT_USER", "User"),
            ("RABBITMQ_DEFAULT_PASS", "Password")
        ]);
            
        _resource = rabbitmq;
    }
}
namespace JackSite.AppHost.Plugins;

public class RedisConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "redis";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("Redis");
        var redis = builder.AddRedis(ResourceName);
            
        // 配置镜像
        redis.WithImage(GetRequiredConfig(section, "ImageName"));
            
        // 添加端点
        AddEndpoint(redis, section, "Port", "redis-port");
            
        _resource = redis;
    }
}
namespace JackSite.AppHost.Plugins;

public class MongoDbConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "mongodb";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("MongoDB");
        var mongodb = builder.AddMongoDB(ResourceName);
            
        // 配置镜像
        mongodb.WithImage(GetRequiredConfig(section, "ImageName"));
            
        // 添加端点
        AddEndpoint(mongodb, section, "Port", "mongodb-port");
            
        // 添加环境变量
        AddEnvironmentVariables(mongodb, section, [
            ("MONGO_INITDB_ROOT_USERNAME", "User"),
            ("MONGO_INITDB_ROOT_PASSWORD", "Password")
        ]);
            
        _resource = mongodb;
    }
}
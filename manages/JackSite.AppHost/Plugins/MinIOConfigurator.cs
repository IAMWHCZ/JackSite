namespace JackSite.AppHost.Plugins;

public class MinIoConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "minio";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("MinIO");
        var minio = builder.AddContainer(ResourceName, GetRequiredConfig(section, "ImageName"));
            
        // 配置卷和参数
        minio.WithVolume("minio-data", "/data")
            .WithArgs("server", "/data", "--console-address", ":9001");
            
        // 添加环境变量
        AddEnvironmentVariables(minio, section, [
            ("MINIO_ROOT_USER", "User"),
            ("MINIO_ROOT_PASSWORD", "Password")
        ]);
            
        // 添加端点
        AddEndpoint(minio, section, "ApiPort", "minio-api");
        AddEndpoint(minio, section, "ConsolePort", "console");
            
        _resource = minio;
    }
}
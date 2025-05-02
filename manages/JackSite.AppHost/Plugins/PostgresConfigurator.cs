namespace JackSite.AppHost.Plugins;

public class PostgresConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "postgres";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("Postgres");
        var postgres = builder.AddPostgres(ResourceName);
            
        // 配置镜像
        postgres.WithImage(GetRequiredConfig(section, "ImageName"));
            
        // 添加端点
        AddEndpoint(postgres, section, "Port", "postgres-port");
            
        // 添加环境变量
        AddEnvironmentVariables(postgres, section, [
            ("POSTGRES_USER", "User"),
            ("POSTGRES_PASSWORD", "Password")
        ]);
            
        _resource = postgres;
    }
}
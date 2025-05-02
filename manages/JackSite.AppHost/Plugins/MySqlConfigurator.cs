namespace JackSite.AppHost.Plugins;

public class MySqlConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "mysql";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("MySQL");

        // 注意：Aspire 可能没有内置的 MySQL 支持，所以这里使用 AddContainer
        var mysql = builder.AddContainer(ResourceName, GetRequiredConfig(section, "ImageName"));

        // 添加端口
        mysql.WithImage(GetRequiredConfig(section, "ImageName"));
        AddEndpoint(mysql, section, "Port", "mysql-port");

        // 添加环境变量
        mysql.WithEnvironment("MYSQL_ROOT_PASSWORD", GetRequiredConfig(section, "Password"))
             .WithEnvironment("MYSQL_USER", GetRequiredConfig(section, "User"));


        // 添加卷以持久化数据
        mysql.WithVolume("mysql-data", "/var/lib/mysql");

        _resource = mysql;
    }
}
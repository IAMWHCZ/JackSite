namespace JackSite.AppHost.Plugins;

public class SqlServerConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "mssql";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        var section = config.GetSection("SqlServer");
        
        // 使用 AddContainer 而不是 AddSqlServer
        var sqlServer = builder.AddContainer(ResourceName, GetRequiredConfig(section, "ImageName"));
        
        // 添加端口映射
        AddEndpoint(sqlServer, section, "Port", "sqlserver-port");
        
        // 添加环境变量
        sqlServer.WithEnvironment("ACCEPT_EULA", "Y")
                 .WithEnvironment("MSSQL_SA_PASSWORD", GetRequiredConfig(section, "Password"))
                 .WithEnvironment("MSSQL_PID", "Developer")
                 .WithEnvironment("MSSQL_AGENT_ENABLED", "true")
                 .WithEnvironment("MSSQL_RUN_AS_ROOT", "1"); // 尝试以 root 用户运行
        
        // 创建并挂载数据卷 - 使用命名卷，并设置为可写
        sqlServer.WithVolume("sqlserver-data", "/var/opt/mssql/data", isReadOnly: false);
        

        _resource = sqlServer;
    }
}
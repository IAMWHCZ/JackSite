namespace JackSite.AppHost.Plugins;

public class SqliteConfigurator : BaseComponentConfigurator
{
    public override string ResourceName => "Sqlite";

    public override void Configure(IDistributedApplicationBuilder builder, IConfiguration config)
    {
        // SQLite 是文件数据库，不需要容器
        // 这里只是为了保持一致性，创建一个虚拟资源
        var section = config.GetSection("SQLite");
        var dbPath = GetRequiredConfig(section, "DbPath");

        // 创建一个虚拟资源
        var sqlite = new SqliteResource(ResourceName, dbPath);
        _resource = sqlite;
    }

    // 自定义 SQLite 资源类
    private class SqliteResource(string name, string dbPath)
    {
        public string Name { get; } = name;
        public string DbPath { get; } = dbPath;
    }
}
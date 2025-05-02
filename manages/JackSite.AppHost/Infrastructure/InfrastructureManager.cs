
using JackSite.AppHost.Interfaces;

namespace JackSite.AppHost.Infrastructure;

public class InfrastructureManager(IDistributedApplicationBuilder builder, IConfiguration config)
{
    private readonly List<IComponentConfigurator> _configurators =
    [
        new RedisConfigurator(),
        new PostgresConfigurator(),
        new MongoDbConfigurator(),
        new RabbitMqConfigurator(),
        new SeqConfigurator(),
        new MinIoConfigurator(),
        new ElasticsearchConfigurator(),
        new PrometheusConfigurator(),
        new GrafanaConfigurator(),
        new SqlServerConfigurator(),
        new MySqlConfigurator(),
        new SqliteConfigurator()
    ];
    private readonly Dictionary<string, object> _resources = new();

    // 初始化所有组件配置器

    public Dictionary<string, object> ConfigureInfrastructure()
    {
        foreach (var configurator in _configurators)
        {
            configurator.Configure(builder, config);
            _resources[configurator.ResourceName] = configurator.Resource;
        }
            
        return _resources;
    }
}
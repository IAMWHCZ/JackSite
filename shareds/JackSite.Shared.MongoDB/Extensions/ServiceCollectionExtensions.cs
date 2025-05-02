namespace JackSite.Shared.MongoDB.Extensions;

/// <summary>
/// 服务集合扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加 MongoDB 服务
    /// </summary>
    public static IServiceCollection AddJackSiteMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        // 配置 MongoDB 设置
        var section = configuration.GetSection("MongoDB");
        services.Configure<MongoDbSettings>(section);
        
        // 注册 MongoDB 客户端工厂
        services.AddSingleton<IMongoDbClientFactory>(provider =>
        {
            var settings = section.Get<MongoDbSettings>() ?? new MongoDbSettings();
            return new MongoDbClientFactory(settings);
        });
        
        return services;
    }
    
    /// <summary>
    /// 添加 MongoDB 仓储
    /// </summary>
    public static IServiceCollection AddMongoRepository<TDocument>(
        this IServiceCollection services,
        string collectionName) where TDocument : Entities.IEntity
    {
        services.AddScoped<IMongoRepository<TDocument>>(provider =>
        {
            var clientFactory = provider.GetRequiredService<IMongoDbClientFactory>();
            return new MongoRepository<TDocument>(clientFactory, collectionName);
        });
        
        return services;
    }
    
    /// <summary>
    /// 添加 MongoDB 索引管理器
    /// </summary>
    public static IServiceCollection AddMongoIndexManager(this IServiceCollection services)
    {
        services.AddScoped<MongoIndexManager>();
        return services;
    }
    
    /// <summary>
    /// 添加 MongoDB 事务管理器
    /// </summary>
    public static IServiceCollection AddMongoTransactionManager(this IServiceCollection services)
    {
        services.AddScoped<MongoTransactionManager>();
        return services;
    }
    
    /// <summary>
    /// 添加 MongoDB 迁移管理器
    /// </summary>
    public static IServiceCollection AddMongoMigrationManager(
        this IServiceCollection services,
        string migrationsCollectionName = "__migrations")
    {
        services.AddScoped<MongoMigrationManager>(provider =>
        {
            var clientFactory = provider.GetRequiredService<IMongoDbClientFactory>();
            return new MongoMigrationManager(clientFactory, migrationsCollectionName);
        });
        
        return services;
    }
    
    /// <summary>
    /// 添加 MongoDB 监控工具
    /// </summary>
    public static IServiceCollection AddMongoDbMonitor(this IServiceCollection services)
    {
        services.AddScoped<MongoDbMonitor>();
        return services;
    }
    
    /// <summary>
    /// 添加 MongoDB 数据导入导出工具
    /// </summary>
    public static IServiceCollection AddMongoImportExport(this IServiceCollection services)
    {
        services.AddScoped<MongoImportExport>();
        return services;
    }
    
    /// <summary>
    /// 添加 MongoDB 分布式缓存
    /// </summary>
    public static IServiceCollection AddMongoDbDistributedCache(this IServiceCollection services)
    {
        services.AddSingleton<IDistributedCache, MongoDbCache>();
        return services;
    }
    
    /// <summary>
    /// 添加所有 MongoDB 服务
    /// </summary>
    public static IServiceCollection AddJackSiteMongoDbFull(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJackSiteMongoDb(configuration);
        services.AddMongoIndexManager();
        services.AddMongoTransactionManager();
        services.AddMongoMigrationManager();
        services.AddMongoDbMonitor();
        services.AddMongoImportExport();
        
        return services;
    }
}
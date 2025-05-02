namespace JackSite.Shared.MongoDB.Migrations;


/// <summary>
/// MongoDB 迁移基类
/// </summary>
public abstract class MongoMigrationBase : IMongoMigration
{
    /// <summary>
    /// 迁移版本
    /// </summary>
    public abstract string Version { get; }
    
    /// <summary>
    /// 迁移名称
    /// </summary>
    public abstract string Name { get; }
    
    /// <summary>
    /// 执行迁移
    /// </summary>
    public abstract Task UpAsync(IMongoDbClientFactory clientFactory, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 回滚迁移
    /// </summary>
    public abstract Task DownAsync(IMongoDbClientFactory clientFactory, CancellationToken cancellationToken = default);
}
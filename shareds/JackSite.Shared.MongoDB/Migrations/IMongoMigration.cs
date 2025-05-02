namespace JackSite.Shared.MongoDB.Migrations;

/// <summary>
/// MongoDB 迁移接口
/// </summary>
public interface IMongoMigration
{
    /// <summary>
    /// 迁移版本
    /// </summary>
    string Version { get; }
    
    /// <summary>
    /// 迁移名称
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// 执行迁移
    /// </summary>
    Task UpAsync(IMongoDbClientFactory clientFactory, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 回滚迁移
    /// </summary>
    Task DownAsync(IMongoDbClientFactory clientFactory, CancellationToken cancellationToken = default);
}

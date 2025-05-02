namespace JackSite.Shared.MongoDB.Migrations;

/// <summary>
/// MongoDB 迁移管理器
/// </summary>
public class MongoMigrationManager(
    IMongoDbClientFactory clientFactory,
    string migrationsCollectionName = "__migrations")
{
    /// <summary>
    /// 执行迁移
    /// </summary>
    public async Task MigrateAsync(IEnumerable<IMongoMigration> migrations, CancellationToken cancellationToken = default)
    {
        // 确保迁移集合存在
        await EnsureMigrationsCollectionExistsAsync(null!,cancellationToken);
        
        // 获取已应用的迁移
        var appliedMigrations = await GetAppliedMigrationsAsync(cancellationToken);
        
        // 按版本排序迁移
        var orderedMigrations = migrations.OrderBy(m => m.Version).ToList();
        
        // 执行未应用的迁移
        foreach (var migration in orderedMigrations.Where(migration => !appliedMigrations.Contains(migration.Version)))
        {
            await ApplyMigrationAsync(migration, cancellationToken);
        }
    }
    
    /// <summary>
    /// 确保迁移集合存在
    /// </summary>
    private async Task EnsureMigrationsCollectionExistsAsync( ListCollectionNamesOptions options,CancellationToken cancellationToken)
    {
        var database = clientFactory.GetDatabase();
        var collections = await database.ListCollectionNamesAsync(options,cancellationToken);
        var collectionsList = await collections.ToListAsync(cancellationToken);
        
        if (!collectionsList.Contains(migrationsCollectionName))
        {
            await database.CreateCollectionAsync(migrationsCollectionName, null, cancellationToken);
        }
    }
    
    /// <summary>
    /// 获取已应用的迁移
    /// </summary>
    private async Task<HashSet<string>> GetAppliedMigrationsAsync(CancellationToken cancellationToken)
    {
        var collection = clientFactory.GetCollection<BsonDocument>(migrationsCollectionName);
        var filter = Builders<BsonDocument>.Filter.Empty;
        var documents = await collection.Find(filter).ToListAsync(cancellationToken);
        
        return [..documents.Select(d => d["Version"].AsString)];
    }
    
    /// <summary>
    /// 应用迁移
    /// </summary>
    private async Task ApplyMigrationAsync(IMongoMigration migration, CancellationToken cancellationToken)
    {
        // 执行迁移
        await migration.UpAsync(clientFactory, cancellationToken);
        
        // 记录迁移
        var collection = clientFactory.GetCollection<BsonDocument>(migrationsCollectionName);
        var document = new BsonDocument
        {
            { "Version", migration.Version },
            { "Name", migration.Name },
            { "AppliedAt", DateTime.UtcNow }
        };
        
        await collection.InsertOneAsync(document, null, cancellationToken);
    }
}


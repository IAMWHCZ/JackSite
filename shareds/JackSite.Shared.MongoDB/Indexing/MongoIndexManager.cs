namespace JackSite.Shared.MongoDB.Indexing;

/// <summary>
/// MongoDB 索引管理器
/// </summary>
public class MongoIndexManager(IMongoDbClientFactory clientFactory)
{
    /// <summary>
    /// 创建索引
    /// </summary>
    public async Task CreateIndexAsync<TDocument>(
        string collectionName,
        string fieldName,
        bool isUnique = false,
        bool isAscending = true,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 创建索引定义
        var indexKeysDefinition = isAscending
            ? Builders<TDocument>.IndexKeys.Ascending(fieldName)
            : Builders<TDocument>.IndexKeys.Descending(fieldName);
            
        // 创建索引模型
        var indexModel = new CreateIndexModel<TDocument>(
            indexKeysDefinition,
            new CreateIndexOptions { Unique = isUnique }
        );
        
        // 创建索引
        await collection.Indexes.CreateOneAsync(indexModel, null, cancellationToken);
    }
    
    /// <summary>
    /// 创建复合索引
    /// </summary>
    public async Task CreateCompoundIndexAsync<TDocument>(
        string collectionName,
        IEnumerable<(string FieldName, bool IsAscending)> fields,
        bool isUnique = false,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 创建索引定义
        var indexKeysDefinitionBuilder = Builders<TDocument>.IndexKeys;
        var indexDefinitions = new List<IndexKeysDefinition<TDocument>>();
        
        foreach (var (fieldName, isAscending) in fields)
        {
            var definition = isAscending
                ? indexKeysDefinitionBuilder.Ascending(fieldName)
                : indexKeysDefinitionBuilder.Descending(fieldName);
                
            indexDefinitions.Add(definition);
        }
        
        var compoundIndex = indexKeysDefinitionBuilder.Combine(indexDefinitions);
        
        // 创建索引模型
        var indexModel = new CreateIndexModel<TDocument>(
            compoundIndex,
            new CreateIndexOptions { Unique = isUnique }
        );
        
        // 创建索引
        await collection.Indexes.CreateOneAsync(indexModel, null, cancellationToken);
    }
    
    /// <summary>
    /// 创建文本索引
    /// </summary>
    public async Task CreateTextIndexAsync<TDocument>(
        string collectionName,
        IEnumerable<string> fieldNames,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 创建索引定义
        var indexKeysDefinitionBuilder = Builders<TDocument>.IndexKeys;
        var indexDefinitions = fieldNames.Select(fieldName => indexKeysDefinitionBuilder.Text(fieldName)).ToList();

        var textIndex = indexKeysDefinitionBuilder.Combine(indexDefinitions);
        
        // 创建索引模型
        var indexModel = new CreateIndexModel<TDocument>(textIndex);
        
        // 创建索引
        await collection.Indexes.CreateOneAsync(indexModel, null, cancellationToken);
    }
    
    /// <summary>
    /// 创建地理空间索引
    /// </summary>
    public async Task CreateGeoSpatialIndexAsync<TDocument>(
        string collectionName,
        string fieldName,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 创建索引定义
        var indexKeysDefinition = Builders<TDocument>.IndexKeys.Geo2DSphere(fieldName);
        
        // 创建索引模型
        var indexModel = new CreateIndexModel<TDocument>(indexKeysDefinition);
        
        // 创建索引
        await collection.Indexes.CreateOneAsync(indexModel, null, cancellationToken);
    }
    
    /// <summary>
    /// 获取集合的所有索引
    /// </summary>
    public async Task<List<string>> GetIndexesAsync<TDocument>(
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 获取索引列表
        var indexes = await collection.Indexes.ListAsync(cancellationToken);
        var indexList = await indexes.ToListAsync(cancellationToken);
        
        return indexList.Select(index => index["name"].AsString).ToList();
    }
    
    /// <summary>
    /// 删除索引
    /// </summary>
    public async Task DropIndexAsync<TDocument>(
        string collectionName,
        string indexName,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 删除索引
        await collection.Indexes.DropOneAsync(indexName, cancellationToken);
    }
    
    /// <summary>
    /// 删除所有索引
    /// </summary>
    public async Task DropAllIndexesAsync<TDocument>(
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        var collection = clientFactory.GetCollection<TDocument>(collectionName);
        
        // 删除所有索引
        await collection.Indexes.DropAllAsync(cancellationToken);
    }
}
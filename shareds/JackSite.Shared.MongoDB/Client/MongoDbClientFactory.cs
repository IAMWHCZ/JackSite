namespace JackSite.Shared.MongoDB.Client;

/// <summary>
/// MongoDB 客户端工厂
/// </summary>
public class MongoDbClientFactory : IMongoDbClientFactory
{
    private readonly MongoDbSettings _settings;
    private readonly Lazy<IMongoClient> _clientLazy;
    private readonly Lazy<IMongoDatabase> _databaseLazy;

    public MongoDbClientFactory(MongoDbSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        
        _clientLazy = new Lazy<IMongoClient>(CreateClient);
        _databaseLazy = new Lazy<IMongoDatabase>(CreateDatabase);
    }

    /// <summary>
    /// 获取 MongoDB 客户端
    /// </summary>
    public IMongoClient GetClient() => _clientLazy.Value;

    /// <summary>
    /// 获取 MongoDB 数据库
    /// </summary>
    public IMongoDatabase GetDatabase() => _databaseLazy.Value;

    /// <summary>
    /// 获取集合
    /// </summary>
    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
    {
        return GetDatabase().GetCollection<TDocument>(collectionName);
    }

    public IMongoClient CreateClient()
    {
        var settings = MongoClientSettings.FromConnectionString(_settings.ConnectionString);
        
        // 配置连接池
        settings.MaxConnectionPoolSize = _settings.MaxConnectionPoolSize;
        
        // 配置超时
        settings.ConnectTimeout = TimeSpan.FromMilliseconds(_settings.ConnectionTimeoutMs);
        settings.ServerSelectionTimeout = TimeSpan.FromMilliseconds(_settings.ServerSelectionTimeoutMs);
        settings.SocketTimeout = TimeSpan.FromMilliseconds(_settings.SocketTimeoutMs);
        
        // 配置直接连接
        settings.DirectConnection = _settings.DirectConnection;
        
        // 配置重试
        settings.RetryWrites = _settings.RetryWrites;
        settings.RetryReads = _settings.RetryReads;
        
        return new MongoClient(settings);
    }

    private IMongoDatabase CreateDatabase()
    {
        return GetClient().GetDatabase(_settings.DatabaseName);
    }
}

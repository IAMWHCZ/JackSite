namespace JackSite.Shared.MongoDB.Cache;

/// <summary>
/// MongoDB 缓存实现
/// </summary>
public class MongoDbCache : IDistributedCache
{
    private readonly IMongoCollection<CacheEntry> _collection;
    private const string CollectionName = "distributed_cache";

    public MongoDbCache(IMongoDbClientFactory clientFactory)
    {
        _collection = clientFactory.GetCollection<CacheEntry>(CollectionName);

        // 创建过期索引
        var indexModel = new CreateIndexModel<CacheEntry>(
            Builders<CacheEntry>.IndexKeys.Ascending(x => x.ExpiresAt),
            new CreateIndexOptions { ExpireAfter = TimeSpan.Zero }
        );

        _collection.Indexes.CreateOne(indexModel);

        // 创建键索引
        var keyIndexModel = new CreateIndexModel<CacheEntry>(
            Builders<CacheEntry>.IndexKeys.Ascending(x => x.Key),
            new CreateIndexOptions { Unique = true }
        );

        _collection.Indexes.CreateOne(keyIndexModel);
    }

    /// <summary>
    /// 获取缓存项
    /// </summary>
    public byte[]? Get(string key)
    {
        var filter = Builders<CacheEntry>.Filter.Eq(x => x.Key, key);
        var entry = _collection.Find(filter).FirstOrDefault();

        return entry?.Value;
    }

    /// <summary>
    /// 异步获取缓存项
    /// </summary>
    public async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        var filter = Builders<CacheEntry>.Filter.Eq(x => x.Key, key);
        var entry = await _collection.Find(filter).FirstOrDefaultAsync(token);

        return entry?.Value;
    }

    /// <summary>
    /// 设置缓存项
    /// </summary>
    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        var entry = new CacheEntry
        {
            Key = key,
            Value = value,
            ExpiresAt = GetExpirationTime(options)
        };

        var filter = Builders<CacheEntry>.Filter.Eq(x => x.Key, key);
        var update = Builders<CacheEntry>.Update
            .Set(x => x.Value, value)
            .Set(x => x.ExpiresAt, entry.ExpiresAt);

        _collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
    }

    /// <summary>
    /// 异步设置缓存项
    /// </summary>
    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
        CancellationToken token = default)
    {
        var entry = new CacheEntry
        {
            Key = key,
            Value = value,
            ExpiresAt = GetExpirationTime(options)
        };

        var filter = Builders<CacheEntry>.Filter.Eq(x => x.Key, key);
        var update = Builders<CacheEntry>.Update
            .Set(x => x.Value, value)
            .Set(x => x.ExpiresAt, entry.ExpiresAt);

        await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, token);
    }

    /// <summary>
    /// 刷新缓存项
    /// </summary>
    public void Refresh(string key)
    {
        // MongoDB TTL 索引不支持刷新，所以这里不做任何操作
    }

    /// <summary>
    /// 异步刷新缓存项
    /// </summary>
    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        // MongoDB TTL 索引不支持刷新，所以这里不做任何操作
        return Task.CompletedTask;
    }

    /// <summary>
    /// 删除缓存项
    /// </summary>
    public void Remove(string key)
    {
        var filter = Builders<CacheEntry>.Filter.Eq(x => x.Key, key);
        _collection.DeleteOne(filter);
    }

    /// <summary>
    /// 异步删除缓存项
    /// </summary>
    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        var filter = Builders<CacheEntry>.Filter.Eq(x => x.Key, key);
        await _collection.DeleteOneAsync(filter, token);
    }

    /// <summary>
    /// 获取过期时间
    /// </summary>
    private DateTime? GetExpirationTime(DistributedCacheEntryOptions options)
    {
        DateTime? expiresAt = null;

        if (options.AbsoluteExpiration.HasValue)
        {
            expiresAt = options.AbsoluteExpiration.Value.UtcDateTime;
        }
        else if (options.AbsoluteExpirationRelativeToNow.HasValue)
        {
            expiresAt = DateTime.UtcNow.Add(options.AbsoluteExpirationRelativeToNow.Value);
        }
        else if (options.SlidingExpiration.HasValue)
        {
            expiresAt = DateTime.UtcNow.Add(options.SlidingExpiration.Value);
        }

        return expiresAt;
    }

    /// <summary>
    /// 缓存条目
    /// </summary>
    private class CacheEntry
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Key { get; set; } = string.Empty;

        public byte[] Value { get; set; } = [];

        public DateTime? ExpiresAt { get; set; }
    }
}
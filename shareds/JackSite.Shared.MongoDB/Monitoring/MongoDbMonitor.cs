namespace JackSite.Shared.MongoDB.Monitoring;

/// <summary>
/// MongoDB 监控工具
/// </summary>
public class MongoDbMonitor(IMongoDbClientFactory clientFactory)
{
    /// <summary>
    /// 获取数据库状态
    /// </summary>
    public async Task<BsonDocument> GetDatabaseStatsAsync(CancellationToken cancellationToken = default)
    {
        var database = clientFactory.GetDatabase();
        var command = new BsonDocument("dbStats", 1);
        return await database.RunCommandAsync<BsonDocument>(command, null, cancellationToken);
    }
    
    /// <summary>
    /// 获取集合状态
    /// </summary>
    public async Task<BsonDocument> GetCollectionStatsAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        var database = clientFactory.GetDatabase();
        var command = new BsonDocument("collStats", collectionName);
        return await database.RunCommandAsync<BsonDocument>(command, null, cancellationToken);
    }
    
    /// <summary>
    /// 获取服务器状态
    /// </summary>
    public async Task<BsonDocument> GetServerStatusAsync(CancellationToken cancellationToken = default)
    {
        var database = clientFactory.GetDatabase();
        var command = new BsonDocument("serverStatus", 1);
        return await database.RunCommandAsync<BsonDocument>(command, null, cancellationToken);
    }
    
    /// <summary>
    /// 获取当前操作
    /// </summary>
    public async Task<List<BsonDocument>> GetCurrentOperationsAsync(CancellationToken cancellationToken = default)
    {
        var database = clientFactory.GetDatabase();
        var command = new BsonDocument("currentOp", 1);
        var result = await database.RunCommandAsync<BsonDocument>(command, null, cancellationToken);
        
        if (result.TryGetValue("inprog", out var inprog) && inprog.IsBsonArray)
        {
            return inprog.AsBsonArray.Select(x => x.AsBsonDocument).ToList();
        }
        
        return [];
    }
    
    /// <summary>
    /// 获取慢查询日志
    /// </summary>
    public async Task<List<BsonDocument>> GetSlowQueriesAsync(CancellationToken cancellationToken = default)
    {
        var database = clientFactory.GetClient().GetDatabase("admin");
        var command = new BsonDocument
        {
            { "find", "system.profile" },
            { "filter", new BsonDocument("op", "query") },
            { "sort", new BsonDocument("millis", -1) },
            { "limit", 10 }
        };
        
        var result = await database.RunCommandAsync<BsonDocument>(command, null, cancellationToken);

        if (!result.TryGetValue("cursor", out var cursor) || !cursor.IsBsonDocument) return [];
        var cursorDoc = cursor.AsBsonDocument;
        if (cursorDoc.TryGetValue("firstBatch", out var firstBatch) && firstBatch.IsBsonArray)
        {
            return firstBatch.AsBsonArray.Select(x => x.AsBsonDocument).ToList();
        }

        return new List<BsonDocument>();
    }
    
    /// <summary>
    /// 获取索引使用情况
    /// </summary>
    public async Task<List<BsonDocument>> GetIndexUsageAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        var database = clientFactory.GetDatabase();
        var command = new BsonDocument
        {
            { "aggregate", collectionName },
            { "pipeline", new BsonArray
                {
                    new BsonDocument("$indexStats", new BsonDocument())
                }
            },
            { "cursor", new BsonDocument() }
        };
        
        var result = await database.RunCommandAsync<BsonDocument>(command, null, cancellationToken);

        if (!result.TryGetValue("cursor", out var cursor) || !cursor.IsBsonDocument) return [];
        var cursorDoc = cursor.AsBsonDocument;
        if (cursorDoc.TryGetValue("firstBatch", out var firstBatch) && firstBatch.IsBsonArray)
        {
            return firstBatch.AsBsonArray.Select(x => x.AsBsonDocument).ToList();
        }

        return [];
    }
}
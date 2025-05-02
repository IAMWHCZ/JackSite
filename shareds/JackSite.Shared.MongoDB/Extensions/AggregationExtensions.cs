namespace JackSite.Shared.MongoDB.Extensions;

/// <summary>
/// 聚合扩展方法
/// </summary>
public static class AggregationExtensions
{
    /// <summary>
    /// 执行聚合管道
    /// </summary>
    public static async Task<List<TResult>> AggregateAsync<TDocument, TResult>(
        this IMongoCollection<TDocument> collection,
        IEnumerable<BsonDocument> pipeline,
        CancellationToken cancellationToken = default)
    {
        var options = new AggregateOptions
        {
            AllowDiskUse = true
        };
        
        var cursor = await collection.AggregateAsync(PipelineDefinition<TDocument, TResult>.Create(pipeline), options, cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }
    
    /// <summary>
    /// 执行聚合管道
    /// </summary>
    public static async Task<List<BsonDocument>> AggregateAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        IEnumerable<BsonDocument> pipeline,
        CancellationToken cancellationToken = default)
    {
        return await AggregateAsync<TDocument, BsonDocument>(collection, pipeline, cancellationToken);
    }
    
    /// <summary>
    /// 分组计数
    /// </summary>
    public static async Task<Dictionary<string, int>> GroupCountAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        string groupField,
        CancellationToken cancellationToken = default)
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", $"${groupField}" },
                { "count", new BsonDocument("$sum", 1) }
            })
        };
        
        var results = await AggregateAsync<TDocument, BsonDocument>(collection, pipeline, cancellationToken);
        
        return results.ToDictionary(
            doc => doc["_id"].ToString()!,
            doc => doc["count"].AsInt32
        );
    }
    
    /// <summary>
    /// 分组求和
    /// </summary>
    public static async Task<Dictionary<string, double>> GroupSumAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        string groupField,
        string sumField,
        CancellationToken cancellationToken = default)
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", $"${groupField}" },
                { "total", new BsonDocument("$sum", $"${sumField}") }
            })
        };
        
        var results = await AggregateAsync<TDocument, BsonDocument>(collection, pipeline, cancellationToken);
        
        return results.ToDictionary(
            doc => doc["_id"].ToString()!,
            doc => doc["total"].AsDouble
        );
    }
    
    /// <summary>
    /// 分组统计
    /// </summary>
    public static async Task<Dictionary<string, BsonDocument>> GroupStatsAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        string groupField,
        string valueField,
        CancellationToken cancellationToken = default)
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", $"${groupField}" },
                { "count", new BsonDocument("$sum", 1) },
                { "avg", new BsonDocument("$avg", $"${valueField}") },
                { "min", new BsonDocument("$min", $"${valueField}") },
                { "max", new BsonDocument("$max", $"${valueField}") },
                { "sum", new BsonDocument("$sum", $"${valueField}") }
            })
        };
        
        var results = await AggregateAsync<TDocument, BsonDocument>(collection, pipeline, cancellationToken);
        
        return results.ToDictionary(
            doc => doc["_id"].ToString()!,
            doc => doc
        );
    }
}
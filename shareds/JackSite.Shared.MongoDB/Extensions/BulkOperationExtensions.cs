namespace JackSite.Shared.MongoDB.Extensions;

/// <summary>
/// 批量操作扩展方法
/// </summary>
public static class BulkOperationExtensions
{
    /// <summary>
    /// 批量插入
    /// </summary>
    public static async Task<BulkWriteResult<TDocument>> BulkInsertAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        IEnumerable<TDocument> documents,
        bool isOrdered = true,
        CancellationToken cancellationToken = default)
    {
        var writeModels = documents.Select(doc => new InsertOneModel<TDocument>(doc));
        
        var options = new BulkWriteOptions
        {
            IsOrdered = isOrdered
        };
        
        return await collection.BulkWriteAsync(writeModels, options, cancellationToken);
    }
    
    /// <summary>
    /// 批量更新
    /// </summary>
    public static async Task<BulkWriteResult<TDocument>> BulkUpdateAsync<TDocument, TField>(
        this IMongoCollection<TDocument> collection,
        IEnumerable<(TField Id, TDocument Document)> updates,
        Expression<Func<TDocument, TField>> idField,
        bool isOrdered = true,
        bool isUpsert = false,
        CancellationToken cancellationToken = default)
    {
        var writeModels = updates.Select(update => 
        {
            var filter = Builders<TDocument>.Filter.Eq(idField, update.Id);
            return new ReplaceOneModel<TDocument>(filter, update.Document) { IsUpsert = isUpsert };
        });
        
        var options = new BulkWriteOptions
        {
            IsOrdered = isOrdered
        };
        
        return await collection.BulkWriteAsync(writeModels, options, cancellationToken);
    }
    
    /// <summary>
    /// 批量删除
    /// </summary>
    public static async Task<BulkWriteResult<TDocument>> BulkDeleteAsync<TDocument, TField>(
        this IMongoCollection<TDocument> collection,
        IEnumerable<TField> ids,
        Expression<Func<TDocument, TField>> idField,
        bool isOrdered = true,
        CancellationToken cancellationToken = default)
    {
        var writeModels = ids.Select(id => 
        {
            var filter = Builders<TDocument>.Filter.Eq(idField, id);
            return new DeleteOneModel<TDocument>(filter);
        });
        
        var options = new BulkWriteOptions
        {
            IsOrdered = isOrdered
        };
        
        return await collection.BulkWriteAsync(writeModels, options, cancellationToken);
    }
    
    /// <summary>
    /// 批量软删除
    /// </summary>
    public static async Task<BulkWriteResult<TDocument>> BulkSoftDeleteAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        IEnumerable<string> ids,
        string? deletedBy = null,
        bool isOrdered = true,
        CancellationToken cancellationToken = default)
        where TDocument : Entities.AuditableMongoEntityBase
    {
        var writeModels = ids.Select(id => 
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            
            var update = Builders<TDocument>.Update
                .Set(x => x.IsDeleted, true)
                .Set(x => x.DeletedAt, DateTime.UtcNow);
                
            if (deletedBy != null)
            {
                update = update.Set(x => x.DeletedBy, deletedBy);
            }
            
            return new UpdateOneModel<TDocument>(filter, update);
        });
        
        var options = new BulkWriteOptions
        {
            IsOrdered = isOrdered
        };
        
        return await collection.BulkWriteAsync(writeModels, options, cancellationToken);
    }
}
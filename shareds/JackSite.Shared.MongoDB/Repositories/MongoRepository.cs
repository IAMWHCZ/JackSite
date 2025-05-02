namespace JackSite.Shared.MongoDB.Repositories;

/// <summary>
/// MongoDB 仓储实现
/// </summary>
public class MongoRepository<TDocument>(IMongoDbClientFactory clientFactory, string collectionName)
    : IMongoRepository<TDocument>
    where TDocument : IEntity
{
    protected readonly IMongoDbClientFactory ClientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    protected readonly string CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));

    /// <summary>
    /// 获取集合
    /// </summary>
    public IMongoCollection<TDocument> Collection => ClientFactory.GetCollection<TDocument>(CollectionName);

    /// <summary>
    /// 获取所有文档
    /// </summary>
    public async Task<List<TDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据条件获取文档
    /// </summary>
    public async Task<List<TDocument>> GetAsync(Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据 ID 获取文档
    /// </summary>
    public async Task<TDocument> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        if (typeof(TDocument).IsAssignableFrom(typeof(IEntity<string>)))
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
        
        throw new InvalidOperationException("文档类型不支持 string 类型的 ID");
    }

    /// <summary>
    /// 添加文档
    /// </summary>
    public async Task<TDocument> AddAsync(TDocument document, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(document, null, cancellationToken);
        return document;
    }

    /// <summary>
    /// 更新文档
    /// </summary>
    public async Task<bool> UpdateAsync(TDocument document, CancellationToken cancellationToken = default)
    {
        if (document is not IEntity<string> entity) throw new InvalidOperationException("文档类型不支持 string 类型的 ID");
        var filter = Builders<TDocument>.Filter.Eq("_id", entity.Id);
            
        // 如果是可审计实体，更新审计信息
        if (document is AuditableMongoEntityBase auditableEntity)
        {
            auditableEntity.UpdatedAt = DateTime.UtcNow;
        }
            
        var result = await Collection.ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = false }, cancellationToken);
        return result.ModifiedCount > 0;

    }

    /// <summary>
    /// 删除文档
    /// </summary>
    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TDocument>.Filter.Eq("_id", id);
        var result = await Collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// 软删除文档
    /// </summary>
    public async Task<bool> SoftDeleteAsync(string id, string? deletedBy = null, CancellationToken cancellationToken = default)
    {
        // 只有可审计实体才支持软删除
        if (!typeof(AuditableMongoEntityBase).IsAssignableFrom(typeof(TDocument)))
        {
            throw new InvalidOperationException("文档类型不支持软删除");
        }
        
        var filter = Builders<TDocument>.Filter.Eq("_id", id);
        var update = Builders<TDocument>.Update
            .Set("isDeleted", true)
            .Set("deletedAt", DateTime.UtcNow);
            
        if (deletedBy != null)
        {
            update = update.Set("deletedBy", deletedBy);
        }
        
        var result = await Collection.UpdateOneAsync(filter, update, null, cancellationToken);
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    public async Task<(List<TDocument> Items, long Total)> GetPagedAsync(
        Expression<Func<TDocument, bool>>? filter = null,
        Expression<Func<TDocument, object>>? sortBy = null,
        bool ascending = true,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // 构建查询
        var query = filter != null
            ? Collection.Find(filter)
            : Collection.Find(Builders<TDocument>.Filter.Empty);
            
        // 计算总数
        var total = await query.CountDocumentsAsync(cancellationToken);
        
        // 排序
        if (sortBy != null)
        {
            query = ascending
                ? query.SortBy(sortBy)
                : query.SortByDescending(sortBy);
        }
        
        // 分页
        var skip = (page - 1) * pageSize;
        var items = await query.Skip(skip).Limit(pageSize).ToListAsync(cancellationToken);
        
        return (items, total);
    }
}
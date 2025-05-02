

namespace JackSite.Shared.MongoDB.Repositories;

/// <summary>
/// MongoDB 仓储接口
/// </summary>
public interface IMongoRepository<TDocument> where TDocument : IEntity
{
    /// <summary>
    /// 获取集合
    /// </summary>
    IMongoCollection<TDocument> Collection { get; }
    
    /// <summary>
    /// 获取所有文档
    /// </summary>
    Task<List<TDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件获取文档
    /// </summary>
    Task<List<TDocument>> GetAsync(Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据 ID 获取文档
    /// </summary>
    Task<TDocument> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 添加文档
    /// </summary>
    Task<TDocument> AddAsync(TDocument document, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 更新文档
    /// </summary>
    Task<bool> UpdateAsync(TDocument document, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 删除文档
    /// </summary>
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 软删除文档
    /// </summary>
    Task<bool> SoftDeleteAsync(string id, string? deletedBy = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<(List<TDocument> Items, long Total)> GetPagedAsync(
        Expression<Func<TDocument, bool>>? filter = null,
        Expression<Func<TDocument, object>>? sortBy = null,
        bool ascending = true,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}
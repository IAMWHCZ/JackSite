namespace JackSite.Authentication.Repositories;

/// <summary>
/// 可草稿化实体的仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IDraftableRepository<TEntity> : IRepository<TEntity> where TEntity : DraftableEntity
{
    /// <summary>
    /// 获取所有草稿
    /// </summary>
    Task<List<TEntity>> GetAllDraftsAsync();
    
    /// <summary>
    /// 获取所有已发布的实体
    /// </summary>
    Task<List<TEntity>> GetAllPublishedAsync();
    
    /// <summary>
    /// 根据条件获取草稿
    /// </summary>
    Task<List<TEntity>> GetDraftsAsync(Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// 根据条件获取已发布的实体
    /// </summary>
    Task<List<TEntity>> GetPublishedAsync(Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// 发布草稿
    /// </summary>
    Task<TEntity> PublishAsync(long id);
    
    /// <summary>
    /// 将实体标记为草稿
    /// </summary>
    Task<TEntity> MarkAsDraftAsync(long id);
    
    /// <summary>
    /// 获取过期草稿
    /// </summary>
    Task<List<TEntity>> GetExpiredDraftsAsync(TimeSpan maxDraftAge);
    
    /// <summary>
    /// 发布所有过期草稿
    /// </summary>
    Task<int> PublishExpiredDraftsAsync(TimeSpan maxDraftAge);
    
    /// <summary>
    /// 删除所有过期草稿
    /// </summary>
    Task<int> DeleteExpiredDraftsAsync(TimeSpan maxDraftAge);
}
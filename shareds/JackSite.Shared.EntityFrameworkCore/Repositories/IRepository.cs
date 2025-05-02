namespace JackSite.Shared.EntityFrameworkCore.Repositories;

/// <summary>
/// 通用仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IRepository<TEntity, in TKey> where TEntity : class
{
    /// <summary>
    /// 获取所有实体
    /// </summary>
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件获取实体
    /// </summary>
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 添加实体
    /// </summary>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 更新实体
    /// </summary>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 删除实体
    /// </summary>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据ID删除实体
    /// </summary>
    Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 获取查询对象
    /// </summary>
    IQueryable<TEntity> Query();
}
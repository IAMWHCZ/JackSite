namespace JackSite.Authentication.Repositories;

/// <summary>
/// 通用仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IRepository<TEntity> where TEntity : Entity
{
    /// <summary>
    /// 获取所有实体
    /// </summary>
    Task<List<TEntity>> GetAllAsync();
    
    /// <summary>
    /// 根据条件获取实体
    /// </summary>
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    Task<TEntity?> GetByIdAsync(long id);
    
    /// <summary>
    /// 添加实体
    /// </summary>
    Task<TEntity> AddAsync(TEntity entity);
    
    /// <summary>
    /// 批量添加实体
    /// </summary>
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
    
    /// <summary>
    /// 更新实体
    /// </summary>
    Task<TEntity> UpdateAsync(TEntity entity);
    
    /// <summary>
    /// 删除实体
    /// </summary>
    Task DeleteAsync(TEntity entity);
    
    /// <summary>
    /// 根据ID删除实体
    /// </summary>
    Task DeleteByIdAsync(long id);
    
    /// <summary>
    /// 批量删除实体
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    
    /// <summary>
    /// 检查是否存在满足条件的实体
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// 获取满足条件的实体数量
    /// </summary>
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
    
    /// <summary>
    /// 获取分页数据
    /// </summary>
    Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(
        int pageIndex, 
        int pageSize, 
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
}
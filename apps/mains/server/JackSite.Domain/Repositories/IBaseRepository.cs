namespace JackSite.Domain.Repositories;

/// <summary>
/// 基础仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IBaseRepository<TEntity> where TEntity : BaseEntity 
{
    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>实体或null</returns>
    Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据ID集合获取实体集合
    /// </summary>
    /// <param name="ids">ID集合</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>实体集合</returns>
    Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 获取所有实体
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>实体集合</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件查找实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>符合条件的实体集合</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件查找单个实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>符合条件的单个实体或null</returns>
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件查找实体并排序
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="orderBy">排序表达式</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>排序后的实体集合</returns>
    Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件查找实体并包含关联实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="includes">关联实体表达式</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>包含关联实体的实体集合</returns>
    Task<IEnumerable<TEntity>> FindWithIncludesAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件查找实体并排序且包含关联实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="orderBy">排序表达式</param>
    /// <param name="includes">关联实体表达式</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>排序后且包含关联实体的实体集合</returns>
    Task<IEnumerable<TEntity>> FindWithIncludesAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件查找单个实体并包含关联实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="includes">关联实体表达式</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>包含关联实体的单个实体或null</returns>
    Task<TEntity?> FindOneWithIncludesAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据ID获取实体并包含关联实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="includes">关联实体表达式</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>包含关联实体的实体或null</returns>
    Task<TEntity?> GetByIdWithIncludesAsync(
        long id,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 添加实体
    /// </summary>
    /// <param name="entity">要添加的实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>添加后的实体</returns>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 批量添加实体
    /// </summary>
    /// <param name="entities">要添加的实体集合</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>添加后的实体集合</returns>
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">要更新的实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entities">要更新的实体集合</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 部分更新实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="propertyValues">要更新的属性及其值</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task UpdatePartialAsync(long id, Dictionary<string, object> propertyValues, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件部分更新实体
    /// </summary>
    /// <param name="predicate">更新条件</param>
    /// <param name="propertyValues">要更新的属性及其值</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdatePartialAsync(Expression<Func<TEntity, bool>> predicate, Dictionary<string, object> propertyValues, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="entity">要删除的实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据ID删除实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 批量删除实体
    /// </summary>
    /// <param name="entities">要删除的实体集合</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据ID集合批量删除实体
    /// </summary>
    /// <param name="ids">ID集合</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task DeleteRangeAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 根据条件删除实体
    /// </summary>
    /// <param name="predicate">删除条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>删除的实体数量</returns>
    Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 检查是否存在符合条件的实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 检查是否存在指定ID的实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 获取符合条件的实体数量
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>实体数量</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 获取实体总数
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>实体总数</returns>
    Task<int> CountAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="skip">跳过的记录数</param>
    /// <param name="take">获取的记录数</param>
    /// <param name="predicate">查询条件</param>
    /// <param name="orderBy">排序表达式</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
        int skip, 
        int take, 
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 分页查询并包含关联实体
    /// </summary>
    /// <param name="skip">跳过的记录数</param>
    /// <param name="take">获取的记录数</param>
    /// <param name="predicate">查询条件</param>
    /// <param name="orderBy">排序表达式</param>
    /// <param name="includes">关联实体表达式</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedWithIncludesAsync(
        int skip, 
        int take, 
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 获取查询对象
    /// </summary>
    /// <returns>查询对象</returns>
    IQueryable<TEntity> Query();
    
    /// <summary>
    /// 获取包含关联实体的查询对象
    /// </summary>
    /// <param name="includes">关联实体表达式</param>
    /// <returns>包含关联实体的查询对象</returns>
    IQueryable<TEntity> QueryWithIncludes(IEnumerable<Expression<Func<TEntity, object>>> includes);
    
    /// <summary>
    /// 执行原生SQL查询
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>查询结果</returns>
    Task<IEnumerable<TEntity>> ExecuteSqlQueryAsync(string sql, object[] parameters, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 执行原生SQL命令
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>影响的行数</returns>
    Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 批量插入
    /// </summary>
    /// <param name="entities">要插入的实体集合</param>
    /// <param name="batchSize">批次大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>插入的实体数量</returns>
    Task<int> BulkInsertAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="entities">要更新的实体集合</param>
    /// <param name="batchSize">批次大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新的实体数量</returns>
    Task<int> BulkUpdateAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="entities">要删除的实体集合</param>
    /// <param name="batchSize">批次大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>删除的实体数量</returns>
    Task<int> BulkDeleteAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 批量合并（存在则更新，不存在则插入）
    /// </summary>
    /// <param name="entities">要合并的实体集合</param>
    /// <param name="batchSize">批次大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>影响的实体数量</returns>
    Task<int> BulkMergeAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 异步迭代查询结果
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="batchSize">批次大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>异步迭代器</returns>
    IAsyncEnumerable<TEntity> AsAsyncEnumerable(
        Expression<Func<TEntity, bool>>? predicate = null,
        int batchSize = 1000,
        CancellationToken cancellationToken = default);
}

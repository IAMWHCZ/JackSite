namespace JackSite.Authentication.Abstractions.Repositories;

/// <summary>
/// 通用仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IRepository<TEntity> where TEntity : Entity
{
    #region 异步方法

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

    /// <summary>
    /// 开始事务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>事务对象</returns>
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 在事务中执行操作
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作结果</returns>
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken = default);

    /// <summary>
    /// 在事务中执行操作
    /// </summary>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存更改
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>影响的行数</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取首个满足条件的实体
    /// </summary>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 获取单个满足条件的实体（若有多个则抛异常）
    /// </summary>
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 执行原生SQL命令
    /// </summary>
    Task<int> ExecuteSqlAsync(string sql, params object[] parameters);

    /// <summary>
    /// 通过原生SQL查询实体
    /// </summary>
    Task<List<TEntity>> FromSqlAsync(string sql, params object[] parameters);

    /// <summary>
    /// 判断是否为空
    /// </summary>
    Task<bool> IsEmptyAsync();

    /// <summary>
    /// 获取最大值
    /// </summary>
    Task<TResult?> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector);

    /// <summary>
    /// 获取最小值
    /// </summary>
    Task<TResult?> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector);

    /// <summary>
    /// 获取所有主键ID
    /// </summary>
    Task<List<long>> GetAllIdsAsync();

    /// <summary>
    /// 创建或更新实体（主键存在则更新，否则创建）
    /// </summary>
    Task<TEntity> CreateOrUpdateAsync(TEntity entity);

    #endregion

    #region 同步方法

    IQueryable<TEntity> GetQueryable();
    
    /// <summary>
    /// 获取所有实体
    /// </summary>
    List<TEntity> GetAll();

    /// <summary>
    /// 根据条件获取实体
    /// </summary>
    List<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    TEntity? GetById(long id);

    /// <summary>
    /// 添加实体
    /// </summary>
    TEntity Add(TEntity entity);

    /// <summary>
    /// 批量添加实体
    /// </summary>
    IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// 更新实体
    /// </summary>
    TEntity Update(TEntity entity);

    /// <summary>
    /// 删除实体
    /// </summary>
    void Delete(TEntity entity);

    /// <summary>
    /// 根据ID删除实体
    /// </summary>
    void DeleteById(long id);

    /// <summary>
    /// 批量删除实体
    /// </summary>
    void DeleteRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// 检查是否存在满足条件的实体
    /// </summary>
    bool Exists(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 获取满足条件的实体数量
    /// </summary>
    int Count(Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// 获取分页数据
    /// </summary>
    (List<TEntity> Items, int TotalCount) GetPaged(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

    /// <summary>
    /// 开始事务
    /// </summary>
    /// <returns>事务对象</returns>
    ITransaction BeginTransaction();

    /// <summary>
    /// 在事务中执行操作
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="action">要执行的操作</param>
    /// <returns>操作结果</returns>
    TResult ExecuteInTransaction<TResult>(Func<TResult> action);

    /// <summary>
    /// 在事务中执行操作
    /// </summary>
    /// <param name="action">要执行的操作</param>
    void ExecuteInTransaction(Action action);

    /// <summary>
    /// 保存更改
    /// </summary>
    /// <returns>影响的行数</returns>
    int SaveChanges();

    /// <summary>
    /// 获取首个满足条件的实体
    /// </summary>
    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 获取单个满足条件的实体（若有多个则抛异常）
    /// </summary>
    TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 执行原生SQL命令
    /// </summary>
    int ExecuteSql(string sql, params object[] parameters);

    /// <summary>
    /// 通过原生SQL查询实体
    /// </summary>
    List<TEntity> FromSql(string sql, params object[] parameters);

    /// <summary>
    /// 判断是否为空
    /// </summary>
    bool IsEmpty();

    /// <summary>
    /// 获取最大值
    /// </summary>
    TResult? Max<TResult>(Expression<Func<TEntity, TResult>> selector);

    /// <summary>
    /// 获取最小值
    /// </summary>
    TResult? Min<TResult>(Expression<Func<TEntity, TResult>> selector);

    /// <summary>
    /// 获取所有主键ID
    /// </summary>
    List<long> GetAllIds();

    /// <summary>
    /// 创建或更新实体（主键存在则更新，否则创建）
    /// </summary>
    TEntity CreateOrUpdate(TEntity entity);

    #endregion
}
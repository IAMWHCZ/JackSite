using JackSite.Authentication.Infrastructure.Transactions;
using JackSite.Authentication.Interfaces.Repositories;

namespace JackSite.Authentication.Infrastructure.Repositories;

/// <summary>
/// 通用仓储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class Repository<TEntity>(AuthenticationDbContext dbContext) : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    /// <summary>
    /// 获取所有实体
    /// </summary>
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    /// <summary>
    /// 根据条件获取实体
    /// </summary>
    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await DbSet.FindAsync(id);
    }

    /// <summary>
    /// 添加实体
    /// </summary>
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        var entry = await DbSet.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// 批量添加实体
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        var addRangeAsync = entities as TEntity[] ?? entities.ToArray();
        await DbSet.AddRangeAsync(addRangeAsync);
        await dbContext.SaveChangesAsync();
        return addRangeAsync;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 根据ID删除实体
    /// </summary>
    public virtual async Task DeleteByIdAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }

    /// <summary>
    /// 批量删除实体
    /// </summary>
    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 检查是否存在满足条件的实体
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }

    /// <summary>
    /// 获取满足条件的实体数量
    /// </summary>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await DbSet.CountAsync();
        }
        
        return await DbSet.CountAsync(predicate);
    }

    /// <summary>
    /// 获取分页数据
    /// </summary>
    public virtual async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(
        int pageIndex, 
        int pageSize, 
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = DbSet;
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        var totalCount = await query.CountAsync();
        
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            
        return (items, totalCount);
    }
    
    /// <summary>
    /// 开始事务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>事务对象</returns>
    public virtual async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfTransaction(transaction);
    }
    
    /// <summary>
    /// 在事务中执行操作
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作结果</returns>
    public virtual async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken = default)
    {
        // 如果已经在事务中，直接执行操作
        if (dbContext.Database.CurrentTransaction != null)
        {
            return await action();
        }
        
        // 创建执行策略
        var strategy = dbContext.Database.CreateExecutionStrategy();
        
        // 使用执行策略执行事务
        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await action();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
    
    /// <summary>
    /// 在事务中执行操作
    /// </summary>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    public virtual async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        await ExecuteInTransactionAsync(async () =>
        {
            await action();
            return true;
        }, cancellationToken);
    }
    
    /// <summary>
    /// 保存更改
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>影响的行数</returns>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
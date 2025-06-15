using JackSite.Authentication.Abstractions.Repositories;
using AuthenticationDbContext = JackSite.Authentication.Infrastructure.Data.Contexts.AuthenticationDbContext;

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
        return await DbSet
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件获取实体
    /// </summary>
    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
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
        return await DbSet
            .AsNoTracking()
            .AnyAsync(predicate);
    }

    /// <summary>
    /// 获取满足条件的实体数量
    /// </summary>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await DbSet
                .AsNoTracking()
                .CountAsync();
        }

        return await DbSet
            .AsNoTracking()
            .CountAsync(predicate);
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
            query = query
                .AsNoTracking()
                .Where(predicate);
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
    public virtual async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        // 如果已经��事务中，直接执行操作
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
    public virtual async Task ExecuteInTransactionAsync(Func<Task> action,
        CancellationToken cancellationToken = default)
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

    /// <summary>
    /// 获取第一个满足条件的实体
    /// </summary>
    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// 获取唯一满足条件的实体
    /// </summary>
    public virtual async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet
            .AsNoTracking()
            .SingleOrDefaultAsync(predicate);
    }

    /// <summary>
    /// 获取查询对象
    /// </summary>
    public virtual IQueryable<TEntity> AsQueryable()
    {
        return DbSet
            .AsQueryable()
            .AsNoTracking();
    }

    /// <summary>
    /// 执行SQL语句
    /// </summary>
    public virtual async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
    {
        return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    /// <summary>
    /// 从SQL语句获取实体列表
    /// </summary>
    public virtual async Task<List<TEntity>> FromSqlAsync(string sql, params object[] parameters)
    {
        return await DbSet
            .FromSqlRaw(sql, parameters)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// 检查是否为空
    /// </summary>
    public virtual async Task<bool> IsEmptyAsync()
    {
        return !await DbSet
            .AsNoTracking()
            .AnyAsync();
    }

    /// <summary>
    /// 获取最大值
    /// </summary>
    public virtual async Task<TResult?> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return await DbSet
            .AsNoTracking()
            .MaxAsync(selector);
    }

    /// <summary>
    /// 获取最小值
    /// </summary>
    public virtual async Task<TResult?> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return await DbSet
            .AsNoTracking()
            .MinAsync(selector);
    }

    /// <summary>
    /// 获取所有实体的ID
    /// </summary>
    public virtual async Task<List<long>> GetAllIdsAsync()
    {
        return await DbSet
            .AsNoTracking()
            .Select(e => e.Id)
            .ToListAsync();
    }

    /// <summary>
    /// 创建或更新实体（主键存在则更新，否则创建）
    /// </summary>
    public virtual async Task<TEntity> CreateOrUpdateAsync(TEntity entity)
    {
        if (entity.Id == 0 || await DbSet.FindAsync(entity.Id) == null)
        {
            var entry = await DbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return DbSet.AsQueryable();
    }

    public List<TEntity> GetAll()
    {
        return DbSet
            .AsNoTracking()
            .ToList();
    }

    public List<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet
            .AsNoTracking()
            .Where(predicate)
            .ToList();
    }

    public TEntity? GetById(long id)
    {
        return DbSet
            .Find(id);
    }

    public TEntity Add(TEntity entity)
    {
        var entry = DbSet.Add(entity);
        dbContext.SaveChanges();
        return entry.Entity;
    }

    public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
    {
        var entitiesToAdd = entities as TEntity[] ?? entities.ToArray();
        DbSet.AddRange(entitiesToAdd);
        dbContext.SaveChanges();
        return entitiesToAdd;
    }

    public TEntity Update(TEntity entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        dbContext.SaveChanges();
        return entity;
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        dbContext.SaveChanges();
    }

    public void DeleteById(long id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
        dbContext.SaveChanges();
    }

    public bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet
            .AsNoTracking()
            .Any(predicate);
    }

    public int Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return DbSet
                .AsNoTracking()
                .Count();
        }

        return DbSet
            .AsNoTracking()
            .Count(predicate);
    }

    public (List<TEntity> Items, int TotalCount) GetPaged(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = DbSet;

        if (predicate != null)
        {
            query = query
                .AsNoTracking()
                .Where(predicate);
        }

        var totalCount = query.Count();

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var items = query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    public ITransaction BeginTransaction()
    {
        var transaction = dbContext.Database.BeginTransaction();
        return new EfTransaction(transaction);
    }

    public TResult ExecuteInTransaction<TResult>(Func<TResult> action)
    {
        // 如果已经在事务中，直接执行操作
        if (dbContext.Database.CurrentTransaction != null)
        {
            return action();
        }

        // 创建执行策略
        var strategy = dbContext.Database.CreateExecutionStrategy();

        // 使用执行策略执行事务
        return strategy.Execute(() =>
        {
            using var transaction = BeginTransaction();
            try
            {
                var result = action();
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        });
    }

    public void ExecuteInTransaction(Action action)
    {
        ExecuteInTransaction(() =>
        {
            action();
            return true;
        });
    }

    public int SaveChanges()
    {
        return dbContext.SaveChanges();
    }

    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet
            .AsNoTracking()
            .FirstOrDefault(predicate);
    }

    public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet
            .AsNoTracking()
            .SingleOrDefault(predicate);
    }

    public int ExecuteSql(string sql, params object[] parameters)
    {
        return dbContext.Database.ExecuteSqlRaw(sql, parameters);
    }

    public List<TEntity> FromSql(string sql, params object[] parameters)
    {
        return DbSet
            .FromSqlRaw(sql, parameters)
            .AsNoTracking()
            .ToList();
    }

    public bool IsEmpty()
    {
        return !DbSet
            .AsNoTracking()
            .Any();
    }

    public TResult? Max<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return DbSet
            .AsNoTracking()
            .Max(selector);
    }

    public TResult? Min<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return DbSet
            .AsNoTracking()
            .Min(selector);
    }

    public List<long> GetAllIds()
    {
        return DbSet
            .AsNoTracking()
            .Select(e => e.Id)
            .ToList();
    }

    public TEntity CreateOrUpdate(TEntity entity)
    {
        if (entity.Id == 0 || DbSet.Find(entity.Id) == null)
        {
            var entry = DbSet.Add(entity);
            dbContext.SaveChanges();
            return entry.Entity;
        }

        dbContext.Entry(entity).State = EntityState.Modified;
        dbContext.SaveChanges();
        return entity;
    }
}
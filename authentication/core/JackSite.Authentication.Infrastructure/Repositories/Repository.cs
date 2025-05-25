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
}
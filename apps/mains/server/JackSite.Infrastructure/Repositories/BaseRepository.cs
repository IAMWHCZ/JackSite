namespace JackSite.Infrastructure.Repositories;

public class BaseRepository<TEntity, TId>(ApplicationDbContext dbContext) 
    : IBaseRepository<TEntity,TId>
    where TEntity : BaseEntity<TId>
    where TId : notnull
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    #region 查询方法

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate).AsNoTracking();
        return await orderBy(query).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindWithIncludesAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate).AsNoTracking();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindWithIncludesAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate).AsNoTracking();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await orderBy(query).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FindOneWithIncludesAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate).AsNoTracking();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdWithIncludesAsync(
        TId id,
        IEnumerable<Expression<Func<TEntity, object>>> includes,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(e => EqualityComparer<TId>.Default.Equals(e.Id, id)).AsNoTracking();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    #endregion

    #region 添加方法

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var entitiesList = entities.ToList();
        await _dbSet.AddRangeAsync(entitiesList, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entitiesList;
    }

    #endregion

    #region 更新方法

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePartialAsync(TId id, Dictionary<string, object> propertyValues, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        var entry = dbContext.Entry(entity);
        foreach (var (property, value) in propertyValues)
        {
            entry.Property(property).CurrentValue = value;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> UpdatePartialAsync(Expression<Func<TEntity, bool>> predicate, Dictionary<string, object> propertyValues, CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        if (entities.Count == 0)
        {
            return 0;
        }

        foreach (var entry in entities.Select(dbContext.Entry))
        {
            foreach (var (property, value) in propertyValues)
            {
                entry.Property(property).CurrentValue = value;
            }
        }

        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region 删除方法

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet.Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken);
        _dbSet.RemoveRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        _dbSet.RemoveRange(entities);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region 检查方法

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().AnyAsync(e => EqualityComparer<TId>.Default.Equals(e.Id, id), cancellationToken);
    }

    #endregion

    #region 计数方法

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate == null
            ? await _dbSet.AsNoTracking().CountAsync(cancellationToken)
            : await _dbSet.AsNoTracking().CountAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().CountAsync(cancellationToken);
    }

    #endregion

    #region 分页方法

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
        int skip,
        int take,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate).AsNoTracking();
        var totalCount = await query.AsNoTracking().CountAsync(cancellationToken);

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var items = await query.AsNoTracking().Skip(skip).Take(take).ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedWithIncludesAsync(
        int skip,
        int take,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null,
        CancellationToken cancellationToken = default)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate).AsNoTracking();
        var totalCount = await query.AsNoTracking().CountAsync(cancellationToken);

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var items = await query.Skip(skip).Take(take).ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    #endregion

    #region 查询对象方法

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsNoTracking();
    }

    public IQueryable<TEntity> QueryWithIncludes(IEnumerable<Expression<Func<TEntity, object>>> includes)
    {
        return includes.Aggregate(_dbSet.AsQueryable(), (current, include) => current.Include(include)).AsNoTracking();
    }

    #endregion

    #region SQL执行方法

    public async Task<IEnumerable<TEntity>> ExecuteSqlQueryAsync(string sql, object[] parameters, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
    }

    public async Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default)
    {
        return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }

    #endregion

    #region 聚合方法

    public async Task<TResult> MaxAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.MaxAsync(selector, cancellationToken);
    }

    public async Task<TResult> MinAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.MinAsync(selector, cancellationToken);
    }

    public async Task<double> AverageAsync(
        Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.AverageAsync(selector, cancellationToken);
    }

    public async Task<int> SumAsync(
        Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.SumAsync(selector, cancellationToken);
    }

    #endregion

    #region 批量操作方法

    public async Task<int> BulkInsertAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default)
    {
        var entitiesList = entities.ToList();
        var totalCount = 0;

        for (var i = 0; i < entitiesList.Count; i += batchSize)
        {
            var batch = entitiesList.Skip(i).Take(batchSize).ToList();
            await _dbSet.AddRangeAsync(batch, cancellationToken);
            totalCount += await dbContext.SaveChangesAsync(cancellationToken);
        }

        return totalCount;
    }

    public async Task<int> BulkUpdateAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default)
    {
        var entitiesList = entities.ToList();
        var totalCount = 0;

        for (var i = 0; i < entitiesList.Count; i += batchSize)
        {
            var batch = entitiesList.Skip(i).Take(batchSize).ToList();
            foreach (var entity in batch)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            totalCount += await dbContext.SaveChangesAsync(cancellationToken);
        }

        return totalCount;
    }

    public async Task<int> BulkDeleteAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default)
    {
        var entitiesList = entities.ToList();
        var totalCount = 0;

        for (var i = 0; i < entitiesList.Count; i += batchSize)
        {
            var batch = entitiesList.Skip(i).Take(batchSize).ToList();
            _dbSet.RemoveRange(batch);
            totalCount += await dbContext.SaveChangesAsync(cancellationToken);
        }

        return totalCount;
    }

    public async Task<int> BulkMergeAsync(
        IEnumerable<TEntity> entities,
        int batchSize = 1000,
        CancellationToken cancellationToken = default)
    {
        var entitiesList = entities.ToList();
        var totalCount = 0;

        for (var i = 0; i < entitiesList.Count; i += batchSize)
        {
            var batch = entitiesList.Skip(i).Take(batchSize).ToList();
            var ids = batch.Select(e => e.Id).ToList();
            var existingEntities = await _dbSet.Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken);
            var existingIds = existingEntities.Select(e => e.Id).ToHashSet();

            var entitiesToAdd = batch.Where(e => !existingIds.Contains(e.Id)).ToList();
            var entitiesToUpdate = batch.Where(e => existingIds.Contains(e.Id)).ToList();

            await _dbSet.AddRangeAsync(entitiesToAdd, cancellationToken);

            foreach (var entity in entitiesToUpdate)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            totalCount += await dbContext.SaveChangesAsync(cancellationToken);
        }

        return totalCount;
    }

    #endregion

    #region 异步迭代方法

    public async IAsyncEnumerable<TEntity> AsAsyncEnumerable(
        Expression<Func<TEntity, bool>>? predicate = null,
        int batchSize = 1000,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        for (var i = 0; i < totalCount; i += batchSize)
        {
            var batch = await query.Skip(i).Take(batchSize).ToListAsync(cancellationToken);
            foreach (var entity in batch)
            {
                yield return entity;
            }
        }
    }

    #endregion
}

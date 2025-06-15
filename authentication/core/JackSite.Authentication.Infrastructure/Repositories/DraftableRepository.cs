using JackSite.Authentication.Interfaces.Repositories;
using AuthenticationDbContext = JackSite.Authentication.Infrastructure.Data.Contexts.AuthenticationDbContext;

namespace JackSite.Authentication.Infrastructure.Repositories;

/// <summary>
/// 可草稿化实体仓储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class DraftableRepository<TEntity>(AuthenticationDbContext dbContext)
    : Repository<TEntity>(dbContext), IDraftableRepository<TEntity>
    where TEntity : DraftableEntity
{
    private readonly AuthenticationDbContext _dbContext = dbContext;

    /// <summary>
    /// 获取所有草稿
    /// </summary>
    public virtual async Task<List<TEntity>> GetAllDraftsAsync()
    {
        return await DbSet.Where(e => e.IsDraft).ToListAsync();
    }

    /// <summary>
    /// 获取所有已发布的实体
    /// </summary>
    public virtual async Task<List<TEntity>> GetAllPublishedAsync()
    {
        return await DbSet
            .Where(e => !e.IsDraft)
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件获取草稿
    /// </summary>
    public virtual async Task<List<TEntity>> GetDraftsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(e => e.IsDraft).Where(predicate).ToListAsync();
    }

    /// <summary>
    /// 根据条件获取已发布的实体
    /// </summary>
    public virtual async Task<List<TEntity>> GetPublishedAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(e => !e.IsDraft).Where(predicate).ToListAsync();
    }

    /// <summary>
    /// 发布草稿
    /// </summary>
    public virtual async Task<TEntity> PublishAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found");
        }
        
        entity.Publish();
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// 将实体标记为草稿
    /// </summary>
    public virtual async Task<TEntity> MarkAsDraftAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found");
        }
        
        entity.MarkAsDraft();
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// 获取过期草稿
    /// </summary>
    public virtual async Task<List<TEntity>> GetExpiredDraftsAsync(TimeSpan maxDraftAge)
    {
        var cutoffDate = DateTime.UtcNow - maxDraftAge;
        return await DbSet
            .Where(e => e.IsDraft && e.DraftedOnUtc != null && e.DraftedOnUtc < cutoffDate)
            .ToListAsync();
    }

    /// <summary>
    /// 发布所有过期草稿
    /// </summary>
    public virtual async Task<int> PublishExpiredDraftsAsync(TimeSpan maxDraftAge)
    {
        var expiredDrafts = await GetExpiredDraftsAsync(maxDraftAge);
        
        foreach (var draft in expiredDrafts)
        {
            draft.Publish();
        }
        
        await _dbContext.SaveChangesAsync();
        return expiredDrafts.Count;
    }

    /// <summary>
    /// 删除所有过期草稿
    /// </summary>
    public virtual async Task<int> DeleteExpiredDraftsAsync(TimeSpan maxDraftAge)
    {
        var expiredDrafts = await GetExpiredDraftsAsync(maxDraftAge);
        DbSet.RemoveRange(expiredDrafts);
        await _dbContext.SaveChangesAsync();
        return expiredDrafts.Count;
    }
}
namespace JackSite.Authentication.Infrastructure.Repositories;

/// <summary>
/// BaseLogEntity 仓储实现
/// </summary>
public sealed class BaseLogEntityRepository(AuthenticationDbContext dbContext)
    : Repository<BaseLogEntity>(dbContext), IBaseLogEntityRepository
{
    public async Task<int> CountByStatusCodeAsync(int statusCode)
    {
        return await DbSet
            .AsNoTracking()
            .CountAsync(x => x.StatusCode == statusCode);
    }

    public async Task<List<BaseLogEntity>> GetRecentExceptionsAsync(int count)
    {
        return await DbSet
            .Where(x => x.Exception != null && x.Exception != "")
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<BaseLogEntity> CreateSuccessLogAsync(string? message = null, long elapsedMilliseconds = 0)
    {
        var entity = new BaseLogEntity
        {
            StatusCode = 200,
            Exception = message,
            ElapsedMilliseconds = elapsedMilliseconds
        };
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<BaseLogEntity> CreateFailureLogAsync(string? exception, long elapsedMilliseconds = 0)
    {
        var entity = new BaseLogEntity
        {
            StatusCode = 500,
            Exception = exception,
            ElapsedMilliseconds = elapsedMilliseconds
        };
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<BaseLogEntity> CreateLogAsync(int statusCode, string? exception = null, long elapsedMilliseconds = 0)
    {
        var entity = new BaseLogEntity
        {
            StatusCode = statusCode,
            Exception = exception,
            ElapsedMilliseconds = elapsedMilliseconds
        };
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }
}

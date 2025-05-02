namespace JackSite.Shared.EntityFrameworkCore.Extensions;

/// <summary>
/// IQueryable 扩展方法
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// 分页查询
    /// </summary>
    public static async Task<Models.PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return new Models.PagedResult<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }
}
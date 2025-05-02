namespace JackSite.Shared.EntityFrameworkCore.Models;

/// <summary>
/// 分页结果
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// 当前页码
    /// </summary>
    public int Page { get; set; }
    
    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// 总记录数
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    
    /// <summary>
    /// 是否有上一页
    /// </summary>
    public bool HasPrevious => Page > 1;
    
    /// <summary>
    /// 是否有下一页
    /// </summary>
    public bool HasNext => Page < TotalPages;
    
    /// <summary>
    /// 数据
    /// </summary>
    public List<T> Items { get; set; } = new();
}
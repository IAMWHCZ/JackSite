namespace JackSite.Shared.EntityFrameworkCore.Entities;

/// <summary>
/// 基础实体类
/// </summary>
public abstract class EntityBase<TKey>
{
    /// <summary>
    /// 主键
    /// </summary>
    public TKey Id { get; set; } = default!;
}




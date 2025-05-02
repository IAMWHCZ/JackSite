namespace JackSite.Shared.EntityFrameworkCore.Entities;

/// <summary>
/// 带软删除的基础实体类
/// </summary>
public abstract class SoftDeleteEntityBase<TKey> : AuditableEntityBase<TKey>
{
    /// <summary>
    /// 是否已删除
    /// </summary>
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// 删除者ID
    /// </summary>
    public string? DeletedBy { get; set; }
}
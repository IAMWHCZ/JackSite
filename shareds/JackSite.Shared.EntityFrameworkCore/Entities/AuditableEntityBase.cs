namespace JackSite.Shared.EntityFrameworkCore.Entities;

/// <summary>
/// 带审计信息的基础实体类
/// </summary>
public abstract class AuditableEntityBase<TKey> : EntityBase<TKey>
{
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 创建者ID
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// 最后更新时间
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
    
    /// <summary>
    /// 最后更新者ID
    /// </summary>
    public string? LastModifiedBy { get; set; }
}
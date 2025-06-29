namespace JackSite.Authentication.Base;

/// <summary>
/// 可草稿化实体的基类
/// </summary>
public abstract class DraftableEntity : Entity, IDraftable
{
    /// <summary>
    /// 是否为草稿状态
    /// </summary>
    public bool IsDraft { get; set; }
    
    /// <summary>
    /// 创建草稿的时间
    /// </summary>
    public DateTimeOffset? DraftedOnUtc { get; set; }
    
    /// <summary>
    /// 将实体标记为草稿
    /// </summary>
    public virtual void MarkAsDraft()
    {
        if (!IsDraft)
        {
            IsDraft = true;
            DraftedOnUtc = DateTimeOffset.UtcNow;
        }
    }
    
    /// <summary>
    /// 将草稿发布为正式版本
    /// </summary>
    public virtual void Publish()
    {
        if (!IsDraft) return;
        IsDraft = false;
        DraftedOnUtc = null;
    }
    
    /// <summary>
    /// 检查实体是否为草稿状态
    /// </summary>
    /// <returns>如果是草稿返回true，否则返回false</returns>
    public bool CheckIsDraft()
    {
        return IsDraft;
    }
    
    /// <summary>
    /// 获取草稿创建时间
    /// </summary>
    /// <returns>草稿创建时间，如果不是草稿则返回null</returns>
    public DateTimeOffset? GetDraftTime()
    {
        return IsDraft ? DraftedOnUtc : null;
    }
    
    /// <summary>
    /// 计算草稿已存在的时间
    /// </summary>
    /// <returns>草稿已存在的时间跨度，如果不是草稿则返回null</returns>
    public TimeSpan? GetDraftAge()
    {
        if (!IsDraft || DraftedOnUtc == null)
            return null;
            
        return DateTimeOffset.UtcNow - DraftedOnUtc.Value;
    }
}
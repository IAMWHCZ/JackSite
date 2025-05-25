namespace JackSite.Authentication.Base;

/// <summary>
/// 表示实体可以处于草稿状态
/// </summary>
public interface IDraftable
{
    /// <summary>
    /// 是否为草稿状态
    /// </summary>
    bool IsDraft { get; set; }
    
    /// <summary>
    /// 创建草稿的时间
    /// </summary>
    DateTime? DraftedOnUtc { get; set; }
    
    /// <summary>
    /// 将实体标记为草稿
    /// </summary>
    void MarkAsDraft();
    
    /// <summary>
    /// 将草稿发布为正式版本
    /// </summary>
    void Publish();
    
    /// <summary>
    /// 检查实体是否为草稿状态
    /// </summary>
    /// <returns>如果是草稿返回true，否则返回false</returns>
    bool CheckIsDraft();
    
    /// <summary>
    /// 获取草稿创建时间
    /// </summary>
    /// <returns>草稿创建时间，如果不是草稿则返回null</returns>
    DateTime? GetDraftTime();
    
    /// <summary>
    /// 计算草稿已存在的时间
    /// </summary>
    /// <returns>草稿已存在的时间跨度，如果不是草稿则返回null</returns>
    TimeSpan? GetDraftAge();
}

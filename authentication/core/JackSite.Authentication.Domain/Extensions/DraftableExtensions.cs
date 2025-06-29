namespace JackSite.Authentication.Extensions;

/// <summary>
/// IDraftable接口的扩展方法
/// </summary>
public static class DraftableExtensions
{
    /// <summary>
    /// 检查草稿是否过期
    /// </summary>
    /// <param name="draftable">可草稿化实体</param>
    /// <param name="maxDraftAge">最大草稿保存时间</param>
    /// <returns>如果草稿已过期返回true，否则返回false</returns>
    public static bool IsDraftExpired(this IDraftable draftable, TimeSpan maxDraftAge)
    {
        if (!draftable.IsDraft || draftable.DraftedOnUtc == null)
            return false;
            
        var draftAge = DateTimeOffset.UtcNow - draftable.DraftedOnUtc.Value;
        return draftAge > maxDraftAge;
    }
    
    /// <summary>
    /// 如果草稿已过期，则自动发布
    /// </summary>
    /// <param name="draftable">可草稿化实体</param>
    /// <param name="maxDraftAge">最大草稿保存时间</param>
    /// <returns>如果草稿已过期并被发布返回true，否则返回false</returns>
    public static bool PublishIfExpired(this IDraftable draftable, TimeSpan maxDraftAge)
    {
        if (!draftable.IsDraftExpired(maxDraftAge)) return false;
        draftable.Publish();
        return true;

    }
    
    /// <summary>
    /// 创建草稿的副本
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="draftable">可草稿化实体</param>
    /// <param name="cloneFunc">克隆函数</param>
    /// <returns>草稿副本</returns>
    public static T CreateDraftCopy<T>(this T draftable, Func<T, T> cloneFunc) where T : IDraftable
    {
        var copy = cloneFunc(draftable);
        copy.MarkAsDraft();
        return copy;
    }
    
    /// <summary>
    /// 获取草稿状态的友好描述
    /// </summary>
    /// <param name="draftable">可草稿化实体</param>
    /// <returns>草稿状态描述</returns>
    public static string GetDraftStatusDescription(this IDraftable draftable)
    {
        if (!draftable.IsDraft)
            return "Published";
            
        if (draftable.DraftedOnUtc == null)
            return "Draft (Unknown time)";
            
        var age = DateTimeOffset.UtcNow - draftable.DraftedOnUtc.Value;
        
        if (age.TotalDays >= 1)
            return $"Draft (Created {(int)age.TotalDays} days ago)";
            
        if (age.TotalHours >= 1)
            return $"Draft (Created {(int)age.TotalHours} hours ago)";
            
        if (age.TotalMinutes >= 1)
            return $"Draft (Created {(int)age.TotalMinutes} minutes ago)";
            
        return "Draft (Created just now)";
    }
}
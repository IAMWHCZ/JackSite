namespace JackSite.Authentication.Application.Attributes;

/// <summary>
/// 标记查询为可缓存
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CacheableAttribute : Attribute
{
    /// <summary>
    /// 绝对过期时间（分钟）
    /// </summary>
    public int? AbsoluteExpirationMinutes { get; }
    
    /// <summary>
    /// 滑动过期时间（分钟）
    /// </summary>
    public int? SlidingExpirationMinutes { get; }
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="absoluteExpirationMinutes">绝对过期时间（分钟）</param>
    /// <param name="slidingExpirationMinutes">滑动过期时间（分钟）</param>
    public CacheableAttribute(int? absoluteExpirationMinutes = 30, int? slidingExpirationMinutes = null)
    {
        AbsoluteExpirationMinutes = absoluteExpirationMinutes;
        SlidingExpirationMinutes = slidingExpirationMinutes;
    }
}
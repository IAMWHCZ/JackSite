namespace JackSite.Authentication.Abstractions.Services;

/// <summary>
/// 缓存服务接口
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// 获取缓存项
    /// </summary>
    /// <typeparam name="T">缓存项类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <returns>缓存项，如果不存在则返回默认值</returns>
    Task<T?> GetAsync<T>(string key);
    
    /// <summary>
    /// 设置缓存项
    /// </summary>
    /// <typeparam name="T">缓存项类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="value">缓存值</param>
    /// <param name="absoluteExpiration">绝对过期时间</param>
    /// <param name="slidingExpiration">滑动过期时间</param>
    Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    
    /// <summary>
    /// 移除缓存项
    /// </summary>
    /// <param name="key">缓存键</param>
    Task RemoveAsync(string key);
    
    /// <summary>
    /// 检查缓存项是否存在
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>如果存在则返回true，否则返回false</returns>
    Task<bool> ExistsAsync(string key);
    
    /// <summary>
    /// 刷新缓存项的过期时间
    /// </summary>
    /// <param name="key">缓存键</param>
    Task RefreshAsync(string key);
    
    /// <summary>
    /// 获取或添加缓存项
    /// </summary>
    /// <typeparam name="T">缓存项类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="factory">如果缓存项不存在，用于创建缓存项的工厂方法</param>
    /// <param name="absoluteExpiration">绝对过期时间</param>
    /// <param name="slidingExpiration">滑动过期时间</param>
    /// <returns>缓存项</returns>
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    
    /// <summary>
    /// 清除所有缓存
    /// </summary>
    /// <returns></returns>
    Task ClearAllAsync();

    string BuildCacheKey(params object[] args);
}
namespace JackSite.Shared.CQRS.Behaviors;

/// <summary>
/// 缓存行为
/// </summary>
public class CachingBehavior<TRequest, TResponse>(IDistributedCache cache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
    where TResponse : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 生成缓存键
        var cacheKey = $"CQRS:{typeof(TRequest).Name}:{JsonSerializer.Serialize(request)}";
        
        // 尝试从缓存获取
        var cachedResponse = await cache.GetStringAsync(cacheKey, cancellationToken);
        
        if (!string.IsNullOrEmpty(cachedResponse))
        {
            // 如果缓存存在，反序列化并返回
            return JsonSerializer.Deserialize<TResponse>(cachedResponse)!;
        }
        
        // 执行查询
        var response = await next(cancellationToken);
        
        // 缓存结果（设置 5 分钟过期）
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        
        await cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(response),
            cacheOptions,
            cancellationToken);
            
        return response;
    }
}
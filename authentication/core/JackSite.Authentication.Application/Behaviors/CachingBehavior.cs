namespace JackSite.Authentication.Application.Behaviors;

/// <summary>
/// 缓存行为 - 仅应用于查询
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(ICacheService cacheService, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 只对查询应用缓存，命令不使用缓存
        if (request is not IQuery<TResponse>)
        {
            return await next(cancellationToken);
        }

        var requestType = request.GetType();
        
        // 检查是否有缓存特性

        if (requestType.GetCustomAttributes(typeof(CacheableAttribute), true)
                .FirstOrDefault() is not CacheableAttribute cacheAttribute)
        {
            return await next(cancellationToken);
        }

        // 生成缓存键
        var cacheKey = $"{requestType.Name}_{JsonSerializer.Serialize(request)}";
        
        // 尝试从缓存获取
        var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey);
        
        if (cachedResponse != null)
        {
            _logger.LogDebug("Returning cached result for {RequestType}", requestType.Name);
            return cachedResponse;
        }

        // 执行查询
        var response = await next(cancellationToken);
        
        // 缓存结果
        await _cacheService.SetAsync(
            cacheKey, 
            response, 
            cacheAttribute.AbsoluteExpirationMinutes.HasValue 
                ? TimeSpan.FromMinutes(cacheAttribute.AbsoluteExpirationMinutes.Value) 
                : null,
            cacheAttribute.SlidingExpirationMinutes.HasValue 
                ? TimeSpan.FromMinutes(cacheAttribute.SlidingExpirationMinutes.Value) 
                : null);
        
        _logger.LogDebug("Cached result for {RequestType}", requestType.Name);
        
        return response;
    }
}
using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Abstractions.Services;
using JackSite.Authentication.Entities.Clients;
using Microsoft.Extensions.Caching.Memory;

namespace JackSite.Authentication.Infrastructure.Services;

public class CorsOriginCacheService(IMemoryCache cache, IRepository<ClientCorsOrigin> clientOriginRepository) : ICorsOriginCacheService
{
    private const string CacheKey = "AllowedCorsOrigins";

    public void RefreshCache()
    {
        cache.Remove(CacheKey);
        // 强制重新加载
        _ = GetAllowedOrigins();
    }

    public List<string> GetAllowedOrigins()
    {
        if (!cache.TryGetValue(CacheKey, out List<string>? allowedOrigins))
        {
            allowedOrigins = clientOriginRepository
                .GetQueryable()
                .Where(o => o.Client.Enabled)
                .Select(x => x.Origin)
                .ToList();

            // 设置缓存，5分钟过期
            cache.Set(CacheKey, allowedOrigins, TimeSpan.FromMinutes(5));
        }

        return allowedOrigins ?? [];
    }
}
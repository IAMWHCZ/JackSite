namespace JackSite.Authentication.Abstractions.Services;

public interface ICorsOriginCacheService
{
    void RefreshCache();
    List<string> GetAllowedOrigins();
}

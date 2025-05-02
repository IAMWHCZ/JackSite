namespace JackSite.Shared.EntityFrameworkCore.Extensions;

/// <summary>
/// IServiceCollection 扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加 JackSite EntityFrameworkCore 服务
    /// </summary>
    public static IServiceCollection AddJackSiteEntityFrameworkCore(this IServiceCollection services)
    {
        // 注册通用仓储
        services.AddScoped(typeof(Repositories.IRepository<,>), typeof(Repositories.Repository<,>));
        
        return services;
    }
    
    /// <summary>
    /// 添加数据库上下文
    /// </summary>
    public static IServiceCollection AddJackSiteDbContext<TContext>(
        this IServiceCollection services,
        DbProvider provider,
        string connectionString) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            options.ConfigureByProvider(provider, connectionString);
        });
        
        return services;
    }
}
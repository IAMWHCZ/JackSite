namespace JackSite.Shared.Http.Extensions;

/// <summary>
/// 最小化 API 扩展方法
/// </summary>
public static class MinimalApiExtensions
{
    /// <summary>
    /// 添加最小化 API 服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddMinimalApis(this IServiceCollection services)
    {
        // 注册所有 IMinimalApiEndpoint 实现
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo<IMinimalApiEndpoint>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    /// <summary>
    /// 添加最小化 API 服务（指定程序集）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="assemblies">程序集</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddMinimalApis(this IServiceCollection services, params Assembly[] assemblies)
    {
        // 注册指定程序集中的所有 IMinimalApiEndpoint 实现
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IMinimalApiEndpoint>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    /// <summary>
    /// 映射所有最小化 API 端点
    /// </summary>
    /// <param name="app">Web 应用程序</param>
    /// <returns>Web 应用程序</returns>
    public static WebApplication MapMinimalApiEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IMinimalApiEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(app);
        }

        return app;
    }

    /// <summary>
    /// 映射所有最小化 API 端点（带分组）
    /// </summary>
    /// <param name="app">Web 应用程序</param>
    /// <param name="groupPrefix">分组前缀</param>
    /// <returns>Web 应用程序</returns>
    public static WebApplication MapMinimalApiEndpoints(this WebApplication app, string groupPrefix)
    {
        var endpoints = app.Services.GetServices<IMinimalApiEndpoint>();
        var group = app.MapGroup(groupPrefix);

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(group);
        }

        return app;
    }

    /// <summary>
    /// 映射 API 版本分组
    /// </summary>
    /// <param name="app">Web 应用程序</param>
    /// <param name="version">API 版本</param>
    /// <returns>RouteGroupBuilder</returns>
    public static RouteGroupBuilder MapApiVersionGroup(this WebApplication app, string version)
    {
        return app.MapGroup($"/api/v{version}");
    }

    /// <summary>
    /// 添加通用 API 响应处理
    /// </summary>
    /// <param name="builder">路由组构建器</param>
    /// <returns>路由组构建器</returns>
    public static RouteGroupBuilder WithApiResponseHandling(this RouteGroupBuilder builder)
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            var result = await next(context);

            // 处理不同类型的结果
            return result switch
            {
                IResult apiResult => apiResult,
                null => Results.NoContent(),
                _ => Results.Ok(new ApiResponse<object>(result))
            };
        });

        return builder;
    }

    /// <summary>
    /// 添加认证和授权
    /// </summary>
    /// <param name="builder">路由组构建器</param>
    /// <param name="policyName">策略名称</param>
    /// <returns>路由组构建器</returns>
    public static RouteGroupBuilder WithAuth(this RouteGroupBuilder builder, string? policyName = null)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            builder.RequireAuthorization();
        }
        else
        {
            builder.RequireAuthorization(policyName);
        }

        return builder;
    }

    /// <summary>
    /// 添加缓存控制
    /// </summary>
    /// <param name="builder">路由组构建器</param>
    /// <param name="seconds">缓存秒数</param>
    /// <returns>路由组构建器</returns>
    public static RouteGroupBuilder WithCaching(this RouteGroupBuilder builder, int seconds = 60)
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            var result = await next(context);

            // 添加缓存头
            context.HttpContext.Response.Headers.CacheControl = $"public, max-age={seconds}";

            return result;
        });

        return builder;
    }
}
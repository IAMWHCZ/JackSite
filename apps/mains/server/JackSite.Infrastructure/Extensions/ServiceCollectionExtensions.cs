namespace JackSite.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApplicationDbContext(configuration)
            .AddRepositories()
            .AddServices()
            .AddInterceptors()
            .AddHybridCache(configuration);  // 添加混合缓存服务

        return services;
    }

    private static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("PGSQL"))
                .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // 注册通用仓储
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        // 自动注册所有特定仓储
        var assembly = Assembly.GetExecutingAssembly();

        // 获取所有实现了IBaseRepository<>接口的非抽象类
        var repositoryTypes = assembly.GetTypes()
            .Where(type => !type.IsAbstract && type is { IsInterface: false, IsGenericType: false }
                                            && type.GetInterfaces().Any(i =>
                                                i.IsGenericType && i.GetGenericTypeDefinition() ==
                                                typeof(IBaseRepository<>)))
            .ToList();

        foreach (var repositoryType in repositoryTypes)
        {
            // 获取该仓储实现的所有接口
            var implementedInterfaces = repositoryType.GetInterfaces()
                .Where(i => i != typeof(IBaseRepository<>)
                            && i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(IBaseRepository<>));

            foreach (var interfaceType in implementedInterfaces)
            {
                // 注册接口和实现
                services.AddScoped(interfaceType, repositoryType);
            }

            // 获取该仓储实现的其他自定义接口
            var customInterfaces = repositoryType.GetInterfaces()
                .Where(i => i != typeof(IBaseRepository<>)
                            && (!i.IsGenericType || i.GetGenericTypeDefinition() != typeof(IBaseRepository<>)));

            foreach (var interfaceType in customInterfaces)
            {
                // 注册自定义接口和实现
                services.AddScoped(interfaceType, repositoryType);
            }
        }

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        // 修改这行，提供具体的实现类
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // 自动注册所有服务
        var assembly = Assembly.GetExecutingAssembly();

        // 获取所有服务实现类
        var serviceTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } &&
                           type.Name.EndsWith("Service") &&
                           !type.Name.StartsWith("Base"))
            .ToList();

        foreach (var serviceType in serviceTypes)
        {
            // 查找该服务实现的接口
            var interfaceType = serviceType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{serviceType.Name}" ||
                                     (i.Name.StartsWith('I') && i.Name[1..] == serviceType.Name));

            if (interfaceType != null)
            {
                // 注册接口和实现
                services.AddScoped(interfaceType, serviceType);
            }
            else
            {
                // 如果没有找到对应的接口，直接注册实现类
                services.AddScoped(serviceType);
            }
        }

        return services;
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddScoped<SoftDeleteInterceptor>();

        return services;
    }
}
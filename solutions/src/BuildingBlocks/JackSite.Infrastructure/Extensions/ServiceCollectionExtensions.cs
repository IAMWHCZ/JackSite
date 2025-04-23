using JackSite.Common.Configs;
using JackSite.Common.DependencyInjection;
using JackSite.Infrastructure.Caching;
using JackSite.Infrastructure.Repositories;
using Scrutor;
using StackExchange.Redis;

namespace JackSite.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // 如果没有提供程序集，使用当前程序集
        var assembliesToScan = assemblies.Length == 0
            ? [Assembly.GetExecutingAssembly()]
            : assemblies;

        // 注册 MediatR
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(assembliesToScan); });

        // 单独注册管道行为
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        // 注册仓储和缓存服务
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<ICacheService, CacheService>();

        var redisConfig = configuration.GetSection("Redis").Get<RedisConfig>();
        if (redisConfig != null)
        {
            if (string.IsNullOrEmpty(redisConfig.ConnectionString))
            {
                var exception = new ArgumentNullException(nameof(redisConfig.ConnectionString),
                    "Redis connection string is not configured")
                {
                    HelpLink = null,
                    HResult = 0,
                    Source = null
                };
                throw exception;
            }

            // 添加Redis分布式缓存
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfig.ConnectionString;
                options.InstanceName = redisConfig?.InstanceName ?? "JackSite_";
            });

            // 添加IConnectionMultiplexer
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configOptions = ConfigurationOptions.Parse(redisConfig.ConnectionString);
                configOptions.ConnectTimeout = redisConfig?.ConnectTimeout ?? 5000;
                configOptions.SyncTimeout = redisConfig?.SyncTimeout ?? 5000;
                configOptions.AllowAdmin = redisConfig?.AllowAdmin ?? false;
                configOptions.ConnectRetry = redisConfig?.ConnectRetry ?? 3;
                configOptions.AbortOnConnectFail = redisConfig?.AbortOnConnectFail ?? false;

                return ConnectionMultiplexer.Connect(configOptions);
            });
        }
    }

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var assembliesToScan = assemblies.Length == 0
            ? [Assembly.GetCallingAssembly()]
            : assemblies;

        services.Scan(scan => scan
            // 扫描指定程序集
            .FromAssemblies(assembliesToScan)

            // 注册实现了 ITransientService 接口的类
            .AddClasses(classes => classes.AssignableTo<ITransientService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()

            // 注册实现了 IScopedService 接口的类    
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // 注册实现了 ISingletonService 接口的类    
            .AddClasses(classes => classes.AssignableTo<ISingletonService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()

            // 注册带有 ServiceLifetime 特性的类
            .AddClasses(classes => classes.Where(type =>
                type.GetCustomAttribute<ServiceLifetimeAttribute>()?.Lifetime == ServiceLifetime.Singleton))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithLifetime(GetServiceLifetime(Type.GetType("JackSite.Infrastructure." + assembliesToScan[0])!))

            // 按命名约定注册服务 (以 Service 结尾的类)
            .AddClasses(classes => classes.Where(type =>
                type.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static ServiceLifetime GetServiceLifetime(Type type)
    {
        var attribute = type.GetCustomAttribute<ServiceLifetimeAttribute>();
        return attribute?.Lifetime ?? ServiceLifetime.Scoped;
    }
}
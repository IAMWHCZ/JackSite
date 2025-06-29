using JackSite.Authentication.Abstractions;
using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Infrastructure.Data.Contexts;
using JackSite.Authentication.Infrastructure.Options;
using JackSite.Authentication.Infrastructure.Services;
using JackSite.Authentication.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Minio;

using AuthenticationDbContext = JackSite.Authentication.Infrastructure.Data.Contexts.AuthenticationDbContext;

namespace JackSite.Authentication.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var snowflakeIdOption = new SnowflakeIdOption();
        var mongoDbOption = new MongoDbOption();
        configuration.GetSection("SnowflakeId").Bind(snowflakeIdOption);
        configuration.GetSection("MongoDB").Bind(mongoDbOption);
        services
            .AddSingleton<MongoDbContext>()
            .AddAllRepositories()
            .AddEntityFrameworkCore(configuration)
            .AddRedisCache(configuration)
            .AddServices();
        
        return services;
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        // 确保 Redis 配置存在
        var redisConfig = configuration.GetSection("RedisCache:Configuration").Value;
        if (string.IsNullOrEmpty(redisConfig))
        {
            throw new ArgumentException("Redis 配置缺失，请检查配置文件中的 RedisCache:Configuration 节点");
        }

        // 添加混合缓存服务
        services.AddHybridCache();
    
        // 使用正确的配置节点名称
        services.AddStackExchangeRedisCache(options =>
        {
            configuration.GetSection("RedisCache").Bind(options);
        });

        // 可选：添加 Redis 连接检查
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(redisConfig);
            // 验证连接
            if (!connection.IsConnected)
            {
                throw new InvalidOperationException($"无法连接到 Redis 服务器: {redisConfig}");
            }
            return connection;
        });

        services.AddScoped<ICacheService, CacheService>();
    
        return services;
    }

    private static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 添加DbContext - 使用 MySql.EntityFrameworkCore
        services.AddDbContext<AuthenticationDbContext>(opt =>
        {
            opt.UseMySQL(configuration.GetConnectionString("MySQL")!);
        });

        services.AddScoped<IApplicationDbInitializer,ApplicationDbInitializer>();
        
        return services;
    }

    /// <summary>
    /// 自动注册所有仓储
    /// </summary>
    private static IServiceCollection AddAllRepositories(this IServiceCollection services)
    {
        // 注册通用仓储
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IDraftableRepository<>), typeof(DraftableRepository<>));

        // 获取所有仓储接口和实现
        var repositoryInterfaceType = typeof(IRepository<>);
        var infrastructureAssembly = typeof(Repository<>).Assembly;
        var domainAssembly = typeof(IRepository<>).Assembly;

        // 查找所有仓储接口
        var repositoryInterfaces = domainAssembly.GetTypes()
            .Where(t => t.IsInterface && t != repositoryInterfaceType &&
                        (t.IsGenericType && t.GetGenericTypeDefinition() == repositoryInterfaceType ||
                         t.GetInterfaces().Any(i =>
                             i.IsGenericType && i.GetGenericTypeDefinition() == repositoryInterfaceType)))
            .ToList();

        // 查找所有仓储实现
        var repositoryImplementations = infrastructureAssembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                        t.GetInterfaces().Any(i =>
                            i.IsGenericType && i.GetGenericTypeDefinition() == repositoryInterfaceType))
            .ToList();

        // 注册所有非泛型仓储
        foreach (var interfaceType in repositoryInterfaces.Where(t => !t.IsGenericType))
        {
            var implementationType = repositoryImplementations.FirstOrDefault(t =>
                interfaceType.IsAssignableFrom(t));

            if (implementationType != null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }

        // 注册所有泛型仓储
        foreach (var interfaceType in repositoryInterfaces.Where(t => t.IsGenericType))
        {
            var implementationType = repositoryImplementations.FirstOrDefault(t =>
                t.IsGenericType &&
                t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType.GetGenericTypeDefinition()));

            if (implementationType != null)
            {
                services.AddScoped(interfaceType.GetGenericTypeDefinition(),
                    implementationType.GetGenericTypeDefinition());
            }
        }

        return services;
    }

    private static IServiceCollection AddMinioConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var minioOption = new MinioOption();
        configuration.GetSection("Minio").Bind(minioOption);
        
        services.AddMinio(opt =>
        {
            opt.WithEndpoint(minioOption.Endpoint)
            .WithCredentials(minioOption.AccessKey, minioOption.SecretKey)
            .WithSSL(minioOption.UseSSL)
            .WithRegion("CN")
            .Build();
        });
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        // 添加服务
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICorsOriginCacheService, CorsOriginCacheService>();
        services.AddScoped<ISecurityService, SecurityService>();
        return services;
    }
}
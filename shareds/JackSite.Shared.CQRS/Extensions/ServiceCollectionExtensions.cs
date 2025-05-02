using FluentValidation;
using JackSite.Shared.CQRS.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JackSite.Shared.CQRS.Extensions;

/// <summary>
/// 服务集合扩展
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加 CQRS 服务
    /// </summary>
    public static IServiceCollection AddJackSiteCQRS(this IServiceCollection services, params Assembly[] assemblies)
    {
        // 注册 MediatR
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssemblies(assemblies);
            
            // 注册行为管道
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        });
        
        // 注册 FluentValidation
        services.AddValidatorsFromAssemblies(assemblies);
        
        return services;
    }
}
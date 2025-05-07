using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace JackSite.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCqrs()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddCqrs(this IServiceCollection services, params Assembly[] assemblies)
    {
        var assembliesToScan = assemblies.Length == 0 
            ? [Assembly.GetExecutingAssembly()]
            : assemblies;
            
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembliesToScan));
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        // 自动注册所有服务
        var assembly = Assembly.GetExecutingAssembly();
        
        // 获取所有服务接口和实现
        var serviceTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && 
                           type.Name.EndsWith("Service") && 
                           !type.Name.StartsWith("Base"))
            .ToList();
        
        foreach (var serviceType in serviceTypes)
        {
            // 查找该服务实现的接口
            var interfaceType = serviceType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
            
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
}
namespace JackSite.Shared.EventBus.Extensions;

/// <summary>
/// 服务集合扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加事件总线
    /// </summary>
    public static IServiceCollection AddJackSiteEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionPath = "EventBus",
        params Assembly[] assemblies)
    {
        // 配置选项
        services.Configure<EventBusOptions>(configuration.GetSection(configSectionPath));
        
        // 添加MassTransit
        services.AddMassTransit(busConfig =>
        {
            // 注册所有事件消费者
            RegisterEventConsumers(busConfig, assemblies);
            
            // 配置RabbitMQ
            busConfig.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<EventBusOptions>>().Value;
                
                // 使用正确的方法重载，将 Port 转换为 ushort
                cfg.Host(options.HostName, (ushort)options.Port, options.VirtualHost, h =>
                {
                    h.Username(options.UserName);
                    h.Password(options.Password);
                    
                    if (options.UseSSL)
                    {
                        h.UseSsl();
                    }
                });
                
                // 配置重试策略
                cfg.UseMessageRetry(r => r.Immediate(options.RetryCount));
                
                // 配置消费者
                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(options.ServiceName, false));
            });
        });
        
        // 注册事件总线
        services.AddSingleton<IEventBus, RabbitMQEventBus>();
        
        return services;
    }
    
    /// <summary>
    /// 注册事件处理器
    /// </summary>
    public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
        where TEvent : class, IEvent
        where TEventHandler : class, IEventHandler<TEvent>
    {
        services.AddScoped<TEventHandler>();
        return services;
    }
    
    /// <summary>
    /// 注册所有事件消费者
    /// </summary>
    private static void RegisterEventConsumers(IBusRegistrationConfigurator busConfig, Assembly[] assemblies)
    {
        // 如果没有提供程序集，使用调用程序集
        if (assemblies.Length == 0)
        {
            assemblies = [Assembly.GetCallingAssembly()];
        }
        
        // 查找所有事件处理器
        var handlerTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            .ToList();
            
        foreach (var registerConsumerMethod in from handlerType in handlerTypes let handlerInterface = handlerType.GetInterfaces()
                     .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)) let eventType = handlerInterface.GetGenericArguments()[0] select typeof(EventConsumer<,>).MakeGenericType(eventType, handlerType) into consumerType select typeof(DependencyInjectionRegistrationExtensions)
                     .GetMethods()
                     .First(m => m.Name == "AddConsumer" && m.GetParameters().Length == 1)
                     .MakeGenericMethod(consumerType))
        {
            registerConsumerMethod.Invoke(null, [busConfig]);
        }
    }
}
using JackSite.Shared.EventBus.Abstractions;
using JackSite.Shared.EventBus.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JackSite.Shared.EventBus.RabbitMQ;

/// <summary>
/// RabbitMQ事件总线实现
/// </summary>
public class RabbitMQEventBus : IEventBus
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMQEventBus> _logger;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public RabbitMQEventBus(
        IBus bus,
        IServiceProvider serviceProvider,
        ILogger<RabbitMQEventBus> logger)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    /// <summary>
    /// 发布事件
    /// </summary>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        try
        {
            _logger.LogInformation("发布事件 {EventType} 到 RabbitMQ", typeof(TEvent).Name);
            await _bus.Publish(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发布事件 {EventType} 失败", typeof(TEvent).Name);
            throw;
        }
    }
    
    /// <summary>
    /// 订阅事件
    /// </summary>
    public void Subscribe<TEvent, TEventHandler>()
        where TEvent : class, IEvent
        where TEventHandler : IEventHandler<TEvent>
    {
        // MassTransit 会自动处理订阅，这里不需要额外实现
        _logger.LogInformation("已订阅事件 {EventType} 使用处理器 {HandlerType}",
            typeof(TEvent).Name, typeof(TEventHandler).Name);
    }
    
    /// <summary>
    /// 取消订阅事件
    /// </summary>
    public void Unsubscribe<TEvent, TEventHandler>()
        where TEvent : class, IEvent
        where TEventHandler : IEventHandler<TEvent>
    {
        // MassTransit 不支持运行时取消订阅，这里只记录日志
        _logger.LogInformation("尝试取消订阅事件 {EventType} 的处理器 {HandlerType}，但 MassTransit 不支持运行时取消订阅",
            typeof(TEvent).Name, typeof(TEventHandler).Name);
    }
}
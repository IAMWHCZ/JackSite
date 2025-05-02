using JackSite.Shared.EventBus.Abstractions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JackSite.Shared.EventBus.RabbitMQ;

/// <summary>
/// 事件消费者
/// </summary>
public class EventConsumer<TEvent, TEventHandler> : IConsumer<TEvent>
    where TEvent : class, IEvent
    where TEventHandler : IEventHandler<TEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventConsumer<TEvent, TEventHandler>> _logger;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public EventConsumer(
        IServiceProvider serviceProvider,
        ILogger<EventConsumer<TEvent, TEventHandler>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    /// <summary>
    /// 消费消息
    /// </summary>
    public async Task Consume(ConsumeContext<TEvent> context)
    {
        _logger.LogInformation("接收到事件 {EventType}，ID: {EventId}", typeof(TEvent).Name, context.Message.Id);
        
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<TEventHandler>();
            await handler.HandleAsync(context.Message, context.CancellationToken);
            
            _logger.LogInformation("事件 {EventType} 处理成功，ID: {EventId}", typeof(TEvent).Name, context.Message.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理事件 {EventType} 失败，ID: {EventId}", typeof(TEvent).Name, context.Message.Id);
            throw;
        }
    }
}
namespace JackSite.Shared.EventBus.Abstractions;

/// <summary>
/// 事件总线接口
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// 发布事件
    /// </summary>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
        
    /// <summary>
    /// 订阅事件
    /// </summary>
    void Subscribe<TEvent, TEventHandler>()
        where TEvent : class, IEvent
        where TEventHandler : IEventHandler<TEvent>;
        
    /// <summary>
    /// 取消订阅事件
    /// </summary>
    void Unsubscribe<TEvent, TEventHandler>()
        where TEvent : class, IEvent
        where TEventHandler : IEventHandler<TEvent>;
}
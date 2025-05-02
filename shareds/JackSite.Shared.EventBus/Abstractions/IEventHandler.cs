namespace JackSite.Shared.EventBus.Abstractions;

/// <summary>
/// 事件处理器接口
/// </summary>
public interface IEventHandler<in TEvent>
    where TEvent : class, IEvent
{
    /// <summary>
    /// 处理事件
    /// </summary>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
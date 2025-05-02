using JackSite.Shared.EventBus.Abstractions;

namespace JackSite.Shared.EventBus.Events;

/// <summary>
/// 基础事件类
/// </summary>
public abstract class EventBase : IEvent
{
    /// <summary>
    /// 事件ID
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationDate { get; }
    
    /// <summary>
    /// 构造函数
    /// </summary>
    protected EventBase()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }
}
namespace JackSite.Shared.EventBus.Abstractions;

/// <summary>
/// 事件接口
/// </summary>
public interface IEvent
{
    /// <summary>
    /// 事件ID
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreationDate { get; }
}
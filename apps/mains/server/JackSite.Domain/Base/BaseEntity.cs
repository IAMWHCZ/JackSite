using System.Collections.Generic;
using JackSite.Domain.Events;

namespace JackSite.Domain.Base;

public abstract class BaseEntity<TId>(TId id)
{
    public TId Id { get; protected set; } = id;

    public long CreateBy { get; set; }
    
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    
    public long UpdateBy { get; set; }
    
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    
    // 领域事件集合
    private List<IDomainEvent>? _domainEvents;

    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();
    
    
    // 添加领域事件
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= [];
        _domainEvents.Add(domainEvent);
    }
    
    // 清除领域事件
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}
using System.Collections.Generic;
using JackSite.Domain.Events;

namespace JackSite.Domain.Base;

public abstract class BaseEntity
{
    public long Id { get; protected set; }

    public long CreateBy { get; set; }
    
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    
    public long UpdateBy { get; set; }
    
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    private static readonly SnowflakeIdGenerator IdGenerator = new(1, 1);
    
    // 领域事件集合
    private List<IDomainEvent>? _domainEvents;
    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();
    
    protected BaseEntity()
    {
        Id = IdGenerator.NextId();
    }
    
    protected BaseEntity(long id)
    {
        Id = id;
    }
    
    // 添加领域事件
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }
    
    // 清除领域事件
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}
namespace JackSite.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
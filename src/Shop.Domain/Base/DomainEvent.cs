namespace Shop.Domain.Core;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime DateOccurred { get; }
    public long Version { get; internal set; }
    public DomainEvent()
    {
        DateOccurred = DateTime.UtcNow;
    }
}

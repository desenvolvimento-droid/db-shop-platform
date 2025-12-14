namespace Shop.Domain.Core;

public abstract class AggregateRoot
{
    public Guid Id { get; protected set; }
    public long Version { get; private set; } = -1;

    private readonly IList<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void LoadFromHistory(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            When(@event);
            Version++;
        }
    }

    protected void ApplyChange(IDomainEvent @event)
    {
        When(@event);
        _domainEvents.Add(@event);
        Version++;
    }

    protected abstract void When(IDomainEvent @event);
}

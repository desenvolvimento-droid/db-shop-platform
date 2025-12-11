using Hangfire;
using Shop.Domain.Core;
using Shop.Domain.Interfaces.Dispatchers;

namespace Shop.Dispatcher.Dispatchers;

public class DomainEventDispatcher(IBackgroundJobClient jobs) : IDomainEventDispatcher
{
    public Task DispatchEventsAsync<T>(T aggregate) where T : AggregateRoot
    {
        foreach (var domainEvent in aggregate.GetDomainEvents())
        {
            jobs.Enqueue<IDomainEventBackgroundQueue>(x => x.ExecuteAsync(domainEvent));
        }

        aggregate.ClearDomainEvents();
        return Task.CompletedTask;
    }
}
using Hangfire;
using MediatR;
using Shop.Domain.Interfaces.Dispatchers;

namespace Shop.Dispatcher.BackgorundQueue;

public class DomainEventBackgroundQueue(IPublisher mediator, IBackgroundJobClient jobs) : IDomainEventBackgroundQueue
{
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public async Task ExecuteAsync(INotification domainEvent)
    {
        await mediator.Publish(domainEvent);
    }
}

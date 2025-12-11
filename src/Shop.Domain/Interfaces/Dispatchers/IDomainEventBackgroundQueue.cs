using MediatR;

namespace Shop.Domain.Interfaces.Dispatchers;

public interface IDomainEventBackgroundQueue
{
    Task ExecuteAsync(INotification domainEvent);
}

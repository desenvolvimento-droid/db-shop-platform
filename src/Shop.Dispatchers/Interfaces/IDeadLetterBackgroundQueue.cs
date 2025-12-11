using MediatR;

namespace Shop.Dispatcher.Interfaces;

public interface IDeadLetterBackgroundQueue
{
    Task ProcessAsync(INotification domainEvent, string errorMessage, string stackTrace);
}
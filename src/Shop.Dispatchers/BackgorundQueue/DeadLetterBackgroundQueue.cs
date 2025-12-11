using MediatR;
using Shop.Dispatcher.Interfaces;

namespace Shop.Dispatcher.BackgorundQueue;

public class DeadLetterBackgroundQueue : IDeadLetterBackgroundQueue
{
    public Task ProcessAsync(INotification domainEvent, string message, string stackTrace)
    {
        // Pode salvar no banco, log, Elastic, etc.
        Console.WriteLine($"DLQ: {domainEvent.GetType().Name}");
        Console.WriteLine($"Error: {message}");
        return Task.CompletedTask;
    }
}

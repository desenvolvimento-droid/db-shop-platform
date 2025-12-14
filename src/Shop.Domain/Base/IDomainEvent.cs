using MediatR;

namespace Shop.Domain.Core
{
    public interface IDomainEvent : INotification
    {
        DateTime DateOccurred { get; }
        Long Version { get; }
    }
}

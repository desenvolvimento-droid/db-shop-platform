using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.Event.EventHandlers
{
    public class UpdateOrderStatusChangedHandler(
        IDbRepository<OrderReadModel> orderDbRepository) : INotificationHandler<OrderStatusChangedEvent>
    {
        private readonly IDbRepository<OrderReadModel> _orderDbRepository = orderDbRepository;

        public async Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            var rm = await _orderDbRepository.GetByIdAsync(notification.Id);
            if (rm == null)
                throw new InvalidOperationException();

            if (rm.LastEventVersion >= notification.Version)
                return;

            rm.OrderStatus = notification.OrderStatus;
            rm.LastEventVersion = notification.Version;

            await _orderDbRepository.UpdateAsync(notification.Id, rm);
        }

    }
}

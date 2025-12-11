using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.Event.EventHandlers
{
    public class PublishOrderStatusChangedEventHandler(
        IDbRepository<OrderReadModel> orderDbRepository) : INotificationHandler<OrderStatusChangedEvent>
    {
        private readonly IDbRepository<OrderReadModel> _orderDbRepository = orderDbRepository;

        public async Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            var orderRm = await _orderDbRepository.GetByIdAsync(notification.Id);
            orderRm.OrderStatus = notification.OrderStatus;
            
            await _orderDbRepository.UpdateAsync(notification.Id, orderRm);
        }
    }
}

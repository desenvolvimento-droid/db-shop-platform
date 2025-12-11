using FluentResults;
using MediatR;
using Shop.Application.Errors;
using Shop.Application.UseCases.Results;
using Shop.Common.Resources;
using Shop.Domain.Aggregates.OrderAggregate;
using Shop.Domain.Interfaces.Dispatchers;
using Shop.Domain.Interfaces.Repositories;

namespace Shop.Application.UseCases.Orders.ChangeOrderStatus
{
    public class ChangeOrderStatusHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IEventStoreRepository eventStoreRepository) : IRequestHandler<ChangeOrderStatusCommand, Result<CommandSucceeded>>
    {
        public async Task<Result<CommandSucceeded>> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await eventStoreRepository.LoadAsync<Order>(request.Id);
            if (order == null)
            {
                return new OrderError(
                    OrderMessages.OrderNotFound,
                    nameof(Order.Id),
                    request.Id);
            }

            switch (request.OrderStatus)
            {
                case OrderStatus.New:
                    order.SetNewStatus();
                    break;
                case OrderStatus.Paid:
                    order.SetPaidStatus();
                    break;
                case OrderStatus.Shipped:
                    order.SetShippedStatus();
                    break;
                case OrderStatus.Cancelled:
                    order.SetCancelledStatus();
                    break;
            }

            await eventStoreRepository.SaveAsync(order);
            await domainEventDispatcher.DispatchEventsAsync(order);

            return new CommandSucceeded(
                order.Id,
                CustomerMessages.CustumerCreatedSucess,
                order.GetDomainEvents().Last().DateOccurred);
        }
    }
}

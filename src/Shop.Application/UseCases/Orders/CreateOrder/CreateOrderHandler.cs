using AutoMapper;
using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Common.Resources;
using Shop.Domain.Aggregates.OrderAggregate;
using Shop.Domain.Aggregates.ProductAggregate;
using Shop.Domain.Interfaces.Dispatchers;
using Shop.Domain.Interfaces.Repositories;

namespace Shop.Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IEventStoreRepository eventStoreRepository,
        IMapper mapper) : IRequestHandler<CreateOrderCommand, Result<CommandSucceeded>>
    {

        public async Task<Result<CommandSucceeded>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);

            foreach (var item in request.OrderItems)
            {
                var product = await eventStoreRepository.LoadAsync<Product>(item.ProductId);

                order.AddOrderItem(item.ProductId, item.Quantity, product.Price);
            }

            await eventStoreRepository.SaveAsync(order);
            await domainEventDispatcher.DispatchEventsAsync(order);

            return new CommandSucceeded(
                order.Id,
                OrderMessages.OrderCreatedSucess,
                order.GetDomainEvents().Last().DateOccurred);
        }
    }
}

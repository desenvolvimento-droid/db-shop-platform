using FluentResults;
using MediatR;
using Shop.Application.Exceptions;
using Shop.Application.UseCases.Results;
using Shop.Common.Resources;
using Shop.Domain.Aggregates.ProductAggregate;
using Shop.Domain.Interfaces.Dispatchers;
using Shop.Domain.Interfaces.Repositories;

namespace Shop.Application.UseCases.Products.UpdateProduct
{
    public class UpdateProductCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IEventStoreRepository eventStoreRepository) : IRequestHandler<UpdateProductCommand, Result<CommandSucceeded>>
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;
        private readonly IEventStoreRepository _eventStoreRepository = eventStoreRepository;

        public async Task<Result<CommandSucceeded>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _eventStoreRepository.LoadAsync<Product>(request.Id);
            if (product == null)
            {
                return new ProductError(ProductMessages.ProductNotFound, request.Id);
            }

            product.Change(request.Name, request.Price);

            await _eventStoreRepository.SaveAsync(product);
            await _domainEventDispatcher.DispatchEventsAsync(product);

            return new CommandSucceeded(
                product.Id,
                ProductMessages.ProductUpdatedSuccessfully,
                product.GetDomainEvents().Last().DateOccurred
            );
        }
    }
}

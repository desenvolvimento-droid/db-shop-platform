using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Common.Resources;
using Shop.Domain.Aggregates.CustomerAggregate;
using Shop.Domain.Interfaces.Dispatchers;
using Shop.Domain.Interfaces.Repositories;

namespace Shop.Application.UseCases.Customers.CreateCustomer
{
    public class CreateCustomerHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IEventStoreRepository eventStoreRepository) : IRequestHandler<CreateCustomerCommand, Result<CommandSucceeded>>

    {
        private readonly IDomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;
        private readonly IEventStoreRepository _eventStoreRepository = eventStoreRepository;

        public async Task<Result<CommandSucceeded>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer(Guid.NewGuid(), request.Name);

            await _eventStoreRepository.SaveAsync(customer);
            await _domainEventDispatcher.DispatchEventsAsync(customer);

            return new CommandSucceeded(
                customer.Id,
                CustomerMessages.CustumerCreatedSucess,
                customer.GetDomainEvents().Last().DateOccurred);
        }
    }
}

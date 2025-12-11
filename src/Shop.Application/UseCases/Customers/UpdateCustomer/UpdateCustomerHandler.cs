using AutoMapper;
using FluentResults;
using MediatR;
using Shop.Application.Exceptions;
using Shop.Application.UseCases.Results;
using Shop.Common.Resources;
using Shop.Domain.Aggregates.CustomerAggregate;
using Shop.Domain.Interfaces.Dispatchers;
using Shop.Domain.Interfaces.Repositories;

namespace Shop.Application.UseCases.Customers.UpdateCustomer
{
    public class UpdateCustomerHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IEventStoreRepository eventStoreRepository,
        IMapper mapper)
        : IRequestHandler<UpdateCustomerCommand, Result<CommandSucceeded>>
    {

        public async Task<Result<CommandSucceeded>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await eventStoreRepository.LoadAsync<Customer>(request.Id);
            if (customer == null)
            {
                return new CustomerError(CustomerMessages.CustumerNotFound, request.Id);
            }

            customer.Change(request.Name);

            await eventStoreRepository.SaveAsync(customer);
            await domainEventDispatcher.DispatchEventsAsync(customer);

            return new CommandSucceeded(
                customer.Id,
                CustomerMessages.CustumerUpdatedSucess,
                customer.GetDomainEvents().Last().DateOccurred);
        }
    }
}

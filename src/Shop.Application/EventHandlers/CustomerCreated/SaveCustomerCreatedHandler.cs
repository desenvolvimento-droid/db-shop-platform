using AutoMapper;
using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.EventHandlers.Customers
{
    public class SaveCustomerCreatedHandler(
        IDbRepository<CustomerReadModel> customerDbRepository,
        IMapper mapper) : INotificationHandler<CustomerCreatedEvent>
    {

        public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (await customerDbRepository.FindByIdAsync(notification.Id) != null)
                return;

            var rm = new CustomerReadModel
            {
                Id = notification.Id,
                Name = notification.Name,
                LastEventVersion = notification.Version
            };

            await customerDbRepository.InsertAsync(rm);
        }

    }
}

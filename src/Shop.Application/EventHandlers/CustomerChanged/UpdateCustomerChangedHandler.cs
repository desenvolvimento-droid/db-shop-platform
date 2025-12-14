using AutoMapper;
using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.EventHandlers.CustomerChanged
{
    public class UpdateCustomerChangedHandler(
        IDbRepository<CustomerReadModel> customerDbRepository,
        IMapper mapper) : INotificationHandler<CustomerChangedEvent>
    {
        public async Task Handle(CustomerChangedEvent notification, CancellationToken cancellationToken)
        {
            var rm = await customerDbRepository.GetByIdAsync(notification.Id);
            if (rm == null)
                throw new InvalidOperationException(); 

            if (rm.LastEventVersion >= notification.Version)
                return;

            rm.Name = notification.Name;
            rm.LastEventVersion = notification.Version;

            await customerDbRepository.UpdateAsync(notification.Id, rm);
        }
    }
}

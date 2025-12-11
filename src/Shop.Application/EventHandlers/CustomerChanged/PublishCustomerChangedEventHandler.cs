using AutoMapper;
using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.Event.Customers.Handlers
{
    public class PublishCustomerChangedEventHandler(
        IDbRepository<CustomerReadModel> customerDbRepository,
        IMapper mapper) : INotificationHandler<CustomerChangedEvent>
    {
        private readonly IDbRepository<CustomerReadModel> _customerDbRepository = customerDbRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(CustomerChangedEvent notification, CancellationToken cancellationToken)
        {
            var customerRm = _mapper.Map<CustomerReadModel>(notification);
            await _customerDbRepository.UpdateAsync(notification.Id, customerRm);
        }
    }
}

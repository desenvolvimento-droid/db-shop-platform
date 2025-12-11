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
        private readonly IDbRepository<CustomerReadModel> _customerDbRepository = customerDbRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var customerRm = _mapper.Map<CustomerReadModel>(notification);
            await _customerDbRepository.InsertAsync(customerRm);
        }
    }
}

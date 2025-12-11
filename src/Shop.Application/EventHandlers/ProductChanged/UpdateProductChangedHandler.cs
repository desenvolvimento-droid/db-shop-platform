using AutoMapper;
using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.Event.Products
{
    public class UpdateProductChangedHandler(
        IDbRepository<ProductReadModel> productDbRepository,
        IMapper mapper) : INotificationHandler<ProductChangedEvent>
    {
        private readonly IDbRepository<ProductReadModel> _productDbRepository = productDbRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(ProductChangedEvent notification, CancellationToken cancellationToken)
        {
            var productRm = _mapper.Map<ProductReadModel>(notification);
            await _productDbRepository.UpdateAsync(notification.Id, productRm);
        }
    }
}

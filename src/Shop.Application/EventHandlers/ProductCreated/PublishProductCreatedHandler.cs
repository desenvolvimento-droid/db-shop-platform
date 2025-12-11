using AutoMapper;
using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.Event.Products
{
    public class PublishProductCreatedHandler(
        IDbRepository<ProductReadModel> productDbRepository,
        IMapper mapper) : INotificationHandler<ProductCreatedEvent>
    {
        private readonly IDbRepository<ProductReadModel> _productDbRepository = productDbRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            var productRm = _mapper.Map<ProductReadModel>(notification);
            await _productDbRepository.InsertAsync(productRm);
        }
    }
}

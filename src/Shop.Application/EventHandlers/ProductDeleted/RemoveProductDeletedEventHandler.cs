using MediatR;
using Shop.Domain.Events;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.Event.ProductDeleted;

public class RemoveProductDeletedEventHandler(
    IDbRepository<ProductReadModel> productDbRepository) : INotificationHandler<ProductDeletedEvent>
{
    private readonly IDbRepository<ProductReadModel> _productDbRepository = productDbRepository;

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _productDbRepository.DeleteAsync(notification.Id);
    }
}

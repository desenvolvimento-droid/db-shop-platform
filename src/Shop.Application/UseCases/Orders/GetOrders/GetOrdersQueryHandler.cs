using AutoMapper;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.UseCases.Orders.GetOrders;

public class GetOrdersQueryHandler(
    IDbRepository<OrderReadModel> orderDbRepository,
    IMapper mapper) : IRequestHandler<GetOrdersQuery, IEnumerable<OrderResult>>
{
    private readonly IDbRepository<OrderReadModel> _orderDbRepository = orderDbRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<OrderResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var ordersRm = await _orderDbRepository.GetAllAsync();
        var ordersDto = _mapper.Map<IEnumerable<OrderResult>>(ordersRm);

        return ordersDto;
    }
}

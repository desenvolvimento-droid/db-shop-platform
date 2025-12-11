using AutoMapper;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.UseCases.Orders.GetOrderById;

public class GetOrderByIdQueryHandler(
    IDbRepository<OrderReadModel> orderDbRepository,
    IMapper mapper) : IRequestHandler<GetOrderByIdQuery, OrderResult>
{
    private readonly IDbRepository<OrderReadModel> _orderDbRepository = orderDbRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<OrderResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderRm = await _orderDbRepository.FindByIdAsync(request.Id);
        var orderDto = _mapper.Map<OrderResult>(orderRm);

        return orderDto;
    }
}

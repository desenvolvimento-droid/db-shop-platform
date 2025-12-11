using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Orders.GetOrderById;

public record class GetOrderByIdQuery(Guid Id) 
    : IRequest<OrderResult>;

using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Orders.GetOrders;

public record class GetOrdersQuery : IRequest<IEnumerable<OrderResult>>;

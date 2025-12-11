using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Domain.Aggregates.OrderAggregate;

namespace Shop.Application.UseCases.Orders.ChangeOrderStatus
{
    public record class ChangeOrderStatusCommand(
        Guid Id,
        OrderStatus OrderStatus) : IRequest<Result<CommandSucceeded>>;
}

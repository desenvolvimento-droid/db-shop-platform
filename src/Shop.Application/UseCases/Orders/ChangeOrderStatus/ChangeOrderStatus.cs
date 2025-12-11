using FluentResults;
using MediatR;
using Shop.Domain.Aggregates.OrderAggregate;

namespace Shop.Application.UseCases.Orders.ChangeOrderStatus
{
    public record class ChangeOrderStatus(
        string Message) : IRequest<Result>;
}

using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Orders.CreateOrder;

public record class CreateOrderCommand(
    Guid CustomerId,
    string City,
    string Street,
    List<CreateOrderItemCommand> OrderItems) : IRequest<Result<CommandSucceeded>>;

public record class CreateOrderItemCommand(
    Guid ProductId,
    int Quantity);

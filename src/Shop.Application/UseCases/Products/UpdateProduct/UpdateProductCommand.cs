using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Products.UpdateProduct;

public record class UpdateProductCommand(
    Guid Id,
    string Name,
    decimal Price) : IRequest<Result<CommandSucceeded>>;

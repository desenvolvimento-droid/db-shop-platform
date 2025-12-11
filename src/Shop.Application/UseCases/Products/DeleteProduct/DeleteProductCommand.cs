using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Products.DeleteProduct;

public record class DeleteProductCommand(Guid Id) 
    : IRequest<Result<CommandSucceeded>>;

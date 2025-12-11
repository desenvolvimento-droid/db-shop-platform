using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Products.CreateProduct;

public record class CreateProductCommand(
    string Name, 
    decimal Price) : IRequest<Result<CommandSucceeded>>;

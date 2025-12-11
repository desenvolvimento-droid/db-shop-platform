using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Products.GetProductById;

public class GetProductByIdQuery(
    Guid id) : IRequest<ProductResult>
{
    public Guid Id { get; } = id;
}

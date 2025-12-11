using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Products.GetProducts;

public record class GetProductsQuery 
    : IRequest<IEnumerable<ProductResult>>;
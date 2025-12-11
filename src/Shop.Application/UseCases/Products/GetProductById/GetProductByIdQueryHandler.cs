using AutoMapper;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.UseCases.Products.GetProductById
{
    public class GetProductByIdQueryHandler(
        IDbRepository<ProductReadModel> productDbRepository,
        IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductResult>
    {
        private readonly IDbRepository<ProductReadModel> _productDbRepository = productDbRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productRm = await _productDbRepository.FindByIdAsync(request.Id);
            var productDto = _mapper.Map<ProductResult>(productRm);

            return productDto;
        }
    }
}

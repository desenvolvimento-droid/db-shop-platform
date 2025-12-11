using AutoMapper;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.UseCases.Products.GetProducts
{
    public class GetProductsQueryHandler(
        IDbRepository<ProductReadModel> productDbRepository,
        IMapper mapper) : IRequestHandler<GetProductsQuery, IEnumerable<ProductResult>>
    {
        private readonly IDbRepository<ProductReadModel> _productDbRepository = productDbRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ProductResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var productsRm = await _productDbRepository.GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<ProductResult>>(productsRm);

            return productsDto;
        }
    }
}

using AutoMapper;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.UseCases.Customers.GetCustomers
{
    public class GetCustomersQueryHandler(
        IDbRepository<CustomerReadModel> customerDbRepository,
        IMapper mapper) : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerResult>>
    {

        public async Task<IEnumerable<CustomerResult>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customersRm = await customerDbRepository.GetAllAsync();
            var customersDto = mapper.Map<IEnumerable<CustomerResult>>(customersRm);

            return customersDto;
        }
    }
}

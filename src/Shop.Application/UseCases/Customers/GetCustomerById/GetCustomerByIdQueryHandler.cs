using AutoMapper;
using MediatR;
using Shop.Application.UseCases.Results;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Application.UseCases.Customers.GetCustomerById
{
    public class GetCustomerByIdQueryHandler(
        IDbRepository<CustomerReadModel> customerDbRepository,
        IMapper mapper) : IRequestHandler<GetCustomerByIdQuery, CustomerResult>
    {
        private readonly IDbRepository<CustomerReadModel> _customerDbRepository = customerDbRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CustomerResult> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customerRm = await _customerDbRepository.FindByIdAsync(request.Id);
            var customerDto = _mapper.Map<CustomerResult>(customerRm);

            return customerDto;
        }
    }
}

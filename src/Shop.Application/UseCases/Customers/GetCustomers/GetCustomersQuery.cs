using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Customers.GetCustomers;

public class GetCustomersQuery : IRequest<IEnumerable<CustomerResult>>;

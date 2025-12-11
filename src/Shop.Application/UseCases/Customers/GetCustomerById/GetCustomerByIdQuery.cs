using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Customers.GetCustomerById;

public record class GetCustomerByIdQuery(Guid Id) 
    : IRequest<CustomerResult>;

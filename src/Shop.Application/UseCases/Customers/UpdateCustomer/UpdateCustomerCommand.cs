using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Customers.UpdateCustomer
{
    public record class UpdateCustomerCommand(Guid Id, string Name) 
        : IRequest<Result<CommandSucceeded>>;
}

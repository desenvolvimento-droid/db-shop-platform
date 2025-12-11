using FluentResults;
using MediatR;
using Shop.Application.UseCases.Results;

namespace Shop.Application.UseCases.Customers.CreateCustomer;

public record class CreateCustomerCommand(string Name) 
    : IRequest<Result<CommandSucceeded>>;

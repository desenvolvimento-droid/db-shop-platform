using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.UseCases.Customers.CreateCustomer;
using Shop.Application.UseCases.Orders.CreateOrder;
using Shop.Application.UseCases.Products.CreateProduct;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.ReadModels;

namespace Shop.Persistence.Persistence;

public static class DatabaseInitializer
{
    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var customerDbRepository = serviceProvider.GetRequiredService<IDbRepository<CustomerReadModel>>();
        var productDbRepository = serviceProvider.GetRequiredService<IDbRepository<ProductReadModel>>();

        if (await customerDbRepository.AnyAsync())
        {
            return;
        }

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        await mediator.Send(
            new CreateCustomerCommand("John Smith"),
            CancellationToken.None);

        await mediator.Send(
            new CreateCustomerCommand("Adam Nowak"),
            CancellationToken.None);

        await mediator.Send(
            new CreateCustomerCommand("Jan Kowalski"),
            CancellationToken.None);

        await mediator.Send(
            new CreateCustomerCommand("Albert Jones"),
            CancellationToken.None);

        await mediator.Send(
            new CreateCustomerCommand("Dominic Johnson"),
            CancellationToken.None);

        await mediator.Send(
            new CreateProductCommand("T-shirt", 10),
            CancellationToken.None);

        await mediator.Send(
            new CreateProductCommand( "Trousers", 20),
            CancellationToken.None);

        await mediator.Send(
            new CreateProductCommand("Umbrella", 30),
            CancellationToken.None);

        await mediator.Send(
            new CreateProductCommand("Jumper", 15),
            CancellationToken.None);

        await mediator.Send(
            new CreateProductCommand("Kettle", 45),
            CancellationToken.None);

        await mediator.Send(
            new CreateProductCommand("Ball", 25),
            CancellationToken.None);

        var customers = await customerDbRepository.GetAllAsync();
        var products = await productDbRepository.GetAllAsync();

        await mediator.Send(
            new CreateOrderCommand(
                customers.ElementAt(0).Id,
                "Warsaw",
                "Wolska 5/4",
                [
                    new(products.ElementAt(0).Id, 1),
                    new(products.ElementAt(1).Id, 2)
                ]),
            CancellationToken.None);

        await mediator.Send(
            new CreateOrderCommand(
                customers.ElementAt(1).Id,
                "Gdansk",
                "Bracka 56/9",
                [
                    new(products.ElementAt(2).Id, 2),
                    new(products.ElementAt(3).Id, 2)
                ]),
            CancellationToken.None);
    }
}

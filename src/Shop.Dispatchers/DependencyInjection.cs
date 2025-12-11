using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Dispatcher.BackgorundQueue;
using Shop.Dispatcher.Dispatchers;
using Shop.Dispatcher.Interfaces;
using Shop.Domain.Interfaces.Dispatchers;

namespace Shop.Dispatcher;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire((sp, config) =>
        {
            config.UseFilter(sp.GetRequiredService<DeadLetterFilter>());
        });


        services.AddSingleton<IDomainEventBackgroundQueue, DomainEventBackgroundQueue>();
        services.AddSingleton<IDeadLetterBackgroundQueue, DeadLetterBackgroundQueue>();
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}

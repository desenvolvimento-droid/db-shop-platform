using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Common.Constants;
using Shop.Domain.Interfaces.Dispatchers;
using Shop.Domain.Interfaces.Repositories;
using Shop.Persistence.Persistence;
using Shop.Persistence.Repositories;

namespace Shop.Dispatcher;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbContext>();
        services.AddTransient(typeof(IDbRepository<>), typeof(MongoDbRepository<>));

        var eventStoreConnectionString = configuration.GetValue<string>($"{SettingConstants.DatabaseSettings}:{SettingConstants.EventStoreConnectionString}");
        services.AddEventStoreClient(eventStoreConnectionString);

        services.AddScoped<IEventStoreRepository, EventStoreRepository>();

        return services;
    }
}

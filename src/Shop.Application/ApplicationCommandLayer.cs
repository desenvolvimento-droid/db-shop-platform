using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shop.Common.Mediator;
using System.Reflection;

namespace Shop.Application.Command;

public static class ApplicationCommandLayer
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

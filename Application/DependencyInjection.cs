using Application.Behavior;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipeLineBehavior<,>));
        
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
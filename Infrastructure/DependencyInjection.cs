using Application.EventBus;
using Application.Products.CreateProduct;
using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<ProductCreatedEventConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var settings = context.GetRequiredService<MessageBrokerSettings>();

                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                configurator.ReceiveEndpoint("product-created-event-queue", e =>
                {
                    e.ConfigureConsumer<ProductCreatedEventConsumer>(context);
                });
            });
        });
        services.AddTransient<IEventBus, EventBus>();
        return services;
    }
}
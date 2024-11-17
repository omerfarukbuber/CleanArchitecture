using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Products.CreateProduct;

public sealed class ProductCreatedEventConsumer(ILogger<ProductCreatedEventConsumer> logger)
    : IConsumer<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventConsumer> _logger = logger;

    public Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        _logger.LogInformation("Product created: {@Product}", context.Message);
        
        return Task.CompletedTask;
    }
}
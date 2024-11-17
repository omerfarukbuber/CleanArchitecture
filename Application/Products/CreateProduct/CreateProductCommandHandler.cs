using Application.Abstractions.Messaging;
using Application.EventBus;
using Domain.Products;
using Domain.Shared.Results;
using Marten;

namespace Application.Products.CreateProduct;

internal sealed class CreateProductCommandHandler(IDocumentSession session, IEventBus eventBus) : ICommandHandler<CreateProductCommand>
{
    private readonly IDocumentSession _session = session;
    private readonly IEventBus _eventBus = eventBus;
    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Tags = request.Tags
        };

        _session.Store(product);

        await _session.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishEvent(new ProductCreatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        }, cancellationToken);

        return Result.Success();
    }
}
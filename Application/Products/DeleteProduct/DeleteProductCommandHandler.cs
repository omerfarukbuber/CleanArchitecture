using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared.Results;
using Marten;

namespace Application.Products.DeleteProduct;

internal sealed class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand>
{
    private readonly IDocumentSession _session = session;

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(Error.NotFound("Product.NotFound",
                $"The product with the Id = '{request.Id}' couldn't found."));
        }

        _session.Delete(product);
        await _session.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
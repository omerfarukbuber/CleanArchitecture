using Application.Abstractions.Messaging;
using Application.Products.GetProducts;
using Domain.Products;
using Domain.Shared;
using Mapster;
using Marten;

namespace Application.Products.UpdateProduct;

internal sealed class UpdateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, ProductResponse>
{
    private readonly IDocumentSession _session = session;

    public async Task<Result<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(request.Id,
                token: cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.Failure(new Error("Product.NotFound", $"The product with the Id = '{request.Id}' couldn't found."));
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Tags = request.Tags;

        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken);

        return Result<ProductResponse>.Success(product.Adapt<ProductResponse>());
    }
}
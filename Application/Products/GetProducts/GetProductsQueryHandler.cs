using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Marten;

namespace Application.Products.GetProducts;

internal sealed class GetProductsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly IDocumentSession _session = session;

    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _session
            .Query<Product>()
            .Select(p => new ProductResponse
            (
                p.Id,
                p.Name,
                p.Price,
                p.Tags
            )).ToListAsync(cancellationToken);

        return Result<List<ProductResponse>>.Success(products.ToList());
    }
}
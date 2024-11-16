using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared.Results;
using Marten;

namespace Application.Products.GetProducts;

internal sealed class GetProductsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly IDocumentSession _session = session;

    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _session
            .Query<Product>()
            .Select(p => new ProductResponse
            (
                p.Id,
                p.Name,
                p.Price,
                p.Tags
            ))
            .OrderByDescending(p => p.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize).ToListAsync(cancellationToken);

        return Result.Success<List<ProductResponse>>(products.ToList());
    }
}
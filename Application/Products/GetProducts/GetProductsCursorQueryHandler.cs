using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared.Results;
using Marten;

namespace Application.Products.GetProducts;

internal sealed class GetProductsCursorQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsCursorQuery, CursorProductResponse>
{
    private readonly IDocumentSession _session = session;

    public async Task<Result<CursorProductResponse>> Handle(GetProductsCursorQuery request, CancellationToken cancellationToken)
    {
        var products = await _session
            .Query<Product>()
            .OrderByDescending(p => p.Id)
            .Where(p => p.Id <= request.Cursor)
            .Take(request.PageSize + 1)
            .Select(p => new ProductResponse
            (
                p.Id,
                p.Name,
                p.Price,
                p.Tags
            )).ToListAsync(cancellationToken);

        var cursor = products.IsEmpty() ? request.Cursor : products.Last().Id;
        var response = new CursorProductResponse(cursor, products.Take(request.PageSize).ToList());

        return Result.Success(response);
    }
}
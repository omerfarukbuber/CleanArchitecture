using Application.Abstractions.Messaging;

namespace Application.Products.GetProducts;

public sealed record GetProductsCursorQuery(long Cursor, int PageSize) : IQuery<CursorProductResponse>;
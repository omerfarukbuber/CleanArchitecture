namespace Application.Products.GetProducts;

public sealed record CursorProductResponse
(
    long Cursor,
    List<ProductResponse> Products
);
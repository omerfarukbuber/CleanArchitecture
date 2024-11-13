using Application.Products.CreateProduct;
using Application.Products.GetProducts;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation;

public class ProductsModule() : CarterModule("api/products")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateProductRequest request, ISender sender) =>
        {
            var createProductCommand = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(createProductCommand);

            return Results.Ok();
        });

        app.MapGet("/", async (ISender sender) =>
        {
            var getProductsQuery = new GetProductsQuery();

            var result = await sender.Send(getProductsQuery);

            return Results.Ok(result);
        });
    }

    public sealed record CreateProductRequest(string Name, decimal Price, List<string> Tags);
}
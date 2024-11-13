using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetProducts;
using Application.Products.UpdateProduct;
using Carter;
using Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Products;

public class ProductsModule() : CarterModule("api/products")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            var getProductsQuery = new GetProductsQuery();

            var result = await sender.Send(getProductsQuery);

            return Results.Ok(result.Data);
        });

        app.MapPost("/", async (CreateProductRequest request, ISender sender) =>
        {
            var createProductCommand = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(createProductCommand);

            return Results.Ok();
        });

        app.MapPut("/{productId:long}",
            async (int productId,
                [FromBody] UpdateProductRequest request,
                ISender sender) =>
        {
            var updateProductCommand = request.Adapt<UpdateProductCommand>() with
            {
                Id = productId,
            };

            var result = await sender.Send(updateProductCommand);

            return result.IsFailure ? Results.NotFound(result) : Results.Ok(result.Data);
        });

        app.MapDelete("/{productId:long}",
            async (int productId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(productId));
            return result.IsFailure ? Results.NotFound(result) : Results.NoContent();
        });
    }
}

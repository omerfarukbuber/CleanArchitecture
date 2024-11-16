using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetProducts;
using Application.Products.UpdateProduct;
using Carter;
using Domain.Shared.Results;
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
        app.MapGet("/", async ([FromQuery]long cursor, [FromQuery] int pageSize, ISender sender) =>
        {
            var getProductsQuery = new GetProductsCursorQuery(cursor <= 0 ? long.MaxValue : cursor , pageSize < 1 ? 20 : pageSize);

            var result = await sender.Send(getProductsQuery);

            return result.Match(Results.Ok, ApiResults.Problem);
        });

        app.MapPost("/", async (CreateProductRequest request, ISender sender) =>
        {
            var createProductCommand = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(createProductCommand);
            return result.Match(Results.NoContent, ApiResults.Problem);
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
            return result.Match(Results.Ok, ApiResults.Problem);
        });

        
        app.MapDelete("/{productId:long}",
            async (int productId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(productId));
            return result.Match(Results.NoContent, ApiResults.Problem);
        });
    }
}

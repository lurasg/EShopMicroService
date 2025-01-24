﻿namespace Catalog.API.Products.GetProductByCategory;

//public record GetProductByCategoryRequest();
public record GetProductByCategoryResponse(IEnumerable<Product> Products);
public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}",
            async (ISender sender, string category) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuey(category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products by Category")
            .WithDescription("Get Products by Category");
    }
}


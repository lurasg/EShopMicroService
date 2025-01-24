﻿using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.EndPoints;

//- Accepts pagination parameters
//- Contstructs a GetOrdersQuery with these parameters
//- Retrieves the data and returns it in a paginated format

//public record GetOrdersRequest(PaginationRequest PaginationRequest);
public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginationRequest paginationRequest,ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersQuery(paginationRequest));

            var response = result.Adapt<GetOrdersResponse>();

            return Results.Ok(response);
        })
        .WithName("GetOrders")
        .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Orders")
        .WithDescription("Get Orders");
    }
}


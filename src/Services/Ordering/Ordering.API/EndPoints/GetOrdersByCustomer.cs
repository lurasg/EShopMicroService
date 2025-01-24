
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.EndPoints;

//- Accepts a customer ID
//- Uses a GetOrdersByCustomerQuery to retrieve the orders
//- Returns the list of orders for that customer

//public record GetOrdersByCustomerRequest(Guid CustomerId);
public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);
public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{CustomerId}", async (Guid CustomerId, ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersByCustomerQuery(CustomerId));

            var response = result.Adapt<GetOrdersByCustomerResponse>();

            return Results.Ok(response);
        })
        .WithName("GetOrdersByCustomer")
        .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Orders By Customer")
        .WithDescription("Get Orders By Customer");
    }
}

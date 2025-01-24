﻿
using Ordering.Application.Orders.Commands.DeleteOrder;
using System.Runtime.InteropServices;

namespace Ordering.API.EndPoints;

//- Accpts the order ID as a parameter
//- Constructs a DeleteOrderCommand
//-  the command using MediatR
//- Returns a success or not found response

//public record DeleteOrderRequest(Guid Id);
public record DeleteOrderResponse(bool IsSuccess);
public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteOrderCommand(id));

            var response = result.Adapt<DeleteOrderResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete order")
        .WithDescription("Delete order");
    }
}

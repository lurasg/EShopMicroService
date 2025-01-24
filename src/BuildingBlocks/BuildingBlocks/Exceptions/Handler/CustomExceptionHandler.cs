using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler (ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurence {time}",
             exception.Message, DateTime.UtcNow);

        (string Details, string Title, int StatusCode) details=exception switch
        { 
        InternalServerException=> 
        (
             exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode=StatusCodes.Status500InternalServerError
        ),
        FluentValidation.ValidationException => 
        (
             exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode = StatusCodes.Status400BadRequest
        ),
        BadRequestException =>
        (
             exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode = StatusCodes.Status400BadRequest
        ),
        NotFoundException =>
        (
             exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode = StatusCodes.Status404NotFound
        ),
        _=>
        (
             exception.Message,
             exception.GetType().Name,  
             context.Response.StatusCode = StatusCodes.Status500InternalServerError
        )
      };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Status = details.StatusCode,
            Detail = details.Details,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if(exception is FluentValidation.ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}

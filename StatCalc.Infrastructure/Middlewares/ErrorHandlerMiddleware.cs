using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using StatCalc.Infrastructure.Exceptions;
using StatCalc.Infrastructure.Models;

namespace StatCalc.Infrastructure.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            await HandleException(context, error);
        }
    }

    private static async Task HandleException(HttpContext context, Exception error)
    {
        HttpStatusCode status;
        var message = error.Message;

        switch (error)
        {
            case BadRequestException:
                status = HttpStatusCode.BadRequest;
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                break;
        }

        var response = context.Response;
        
        response.ContentType = "application/json";
        response.StatusCode = (int)status;
        
        var result = JsonSerializer.Serialize(new ErrorHandlerResponse { Message = message });

        await response.WriteAsync(result);
    }
}

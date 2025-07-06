using System.Text.Json;
using Common;
using KnowledgeBase.Core.ErrorMessages;

namespace KnowledgeBase.API.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        
        try
        {
            return next(context);
        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Set the response status code and content
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var errorResponse = ApiResponse<string>.Failure(GeneralErrorMessages.InternalServerError);
            var response = JsonSerializer.Serialize(errorResponse);

            // Return a generic error message
            return context.Response.WriteAsync(response);
        }
    }
}
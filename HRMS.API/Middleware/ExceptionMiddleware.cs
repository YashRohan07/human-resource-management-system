using FluentValidation;
using HRMS.API.Common;
using HRMS.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace HRMS.API.Middleware;

// Handles all unhandled exceptions globally
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pass request to next middleware
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object>
        {
            Success = false
        };

        switch (exception)
        {
            case ValidationException validationException:

                context.Response.StatusCode =
                    (int)HttpStatusCode.BadRequest;

                response.Message = "Validation failed";

                response.Errors = validationException.Errors
                    .Select(error => error.ErrorMessage);

                break;

            case NotFoundException notFoundException:

                context.Response.StatusCode =
                    notFoundException.StatusCode;

                response.Message = notFoundException.Message;

                break;

            case ConflictException conflictException:

                context.Response.StatusCode =
                    conflictException.StatusCode;

                response.Message = conflictException.Message;

                break;

            case AppException appException:

                context.Response.StatusCode =
                    appException.StatusCode;

                response.Message = appException.Message;

                break;

            default:

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                response.Message =
                    "An unexpected error occurred";

                break;
        }

        var jsonResponse =
            JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(jsonResponse);
    }
}
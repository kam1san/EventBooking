using Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = JsonSerializer.Serialize(new
                {
                    status = (int)HttpStatusCode.BadRequest,
                    errors = validationException.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    })
                });

                return context.Response.WriteAsync(response);
            }

            var (statusCode, message) = exception switch
            {
                NotFoundException ex => (HttpStatusCode.NotFound, ex.Message),
                DomainException ex => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = JsonSerializer.Serialize(new
            {
                status = (int)statusCode,
                error = message
            });

            return context.Response.WriteAsync(errorResponse);
        }
    }
}
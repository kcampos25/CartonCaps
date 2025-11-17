using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;


namespace CartonCaps.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var code = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            //FluentValidation error handling
            if (exception is FluentValidation.ValidationException validationException)
            {
                code = HttpStatusCode.BadRequest;
                var validationErrors = new Dictionary<string, string>();

                foreach (var error in validationException.Errors)
                {
                    validationErrors[error.PropertyName] = error.ErrorMessage;
                }

                var validationResult = JsonSerializer.Serialize(new
                {
                    code = code,
                    message = "Validation failed",
                    errors = validationErrors,
                    traceId = context.TraceIdentifier
                });

                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(validationResult);
            }
            //controlled errors
            else if (exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is KeyNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                message = exception.Message;
            }

            //Generic error
            context.Response.StatusCode = (int)code;

            var result = JsonSerializer.Serialize(new
            {
                code = code,
                message = message,
                traceId = context.TraceIdentifier
            });

            return context.Response.WriteAsync(result);
        }
    }
}

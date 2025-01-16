using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace CarStockAPI.Middlewares
{
    public class ExceptionHandling
    {
        private readonly ILogger<ExceptionHandling> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandling(ILogger<ExceptionHandling> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// основная точка входа middleware
        /// </summary>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Key not found error: {Message}", ex.Message);
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new { error = ex.Message });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhadled exception occurred");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var status = HttpStatusCode.InternalServerError;

            if (exception is ArgumentException)
            {
                status = HttpStatusCode.BadRequest;
            }

            var response = new
            {
                Error = exception.Message,
                Type = exception.GetType().Name,
                Details = "An error occurred while processing your request"
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)status;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            };

            return httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
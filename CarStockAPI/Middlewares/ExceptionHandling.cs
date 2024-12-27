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

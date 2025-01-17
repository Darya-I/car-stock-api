using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using CarStockBLL.CustomException;

namespace CarStockAPI.Middlewares
{
    public abstract class AbstractExceptonMiddleware
    {
        private readonly ILogger<AbstractExceptonMiddleware> _logger;
        private readonly RequestDelegate _next;

        public AbstractExceptonMiddleware(ILogger<AbstractExceptonMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Ловит ошибки из бизнес слоя
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>Код и сообщение об ошибке</returns>
        public abstract (HttpStatusCode code, string message) GetException(ApiException exception);

        /// <summary>
        /// Основная точка входа middleware. Обрабатывает исключения
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
            catch (ApiException ex)
            {
                _logger.LogError(ex, "An unhadled exception occurred");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Обрабатывает исключение и формирует стандартный ответ для клиента
        /// </summary>
        /// <param name="httpContext">Контекст запросаparam>
        /// <param name="exception">Исключение</param>
        /// <returns>Задача, представляющая асинхронную операцию записи ответа</returns>
        public static Task HandleExceptionAsync(HttpContext httpContext, ApiException apiException)
        {
            var status = HttpStatusCode.InternalServerError;

            //if (exception is ArgumentException)
            //{
            //    status = HttpStatusCode.BadRequest;
            //}

            var response = new
            {
                Error = apiException.Message,
                Type = apiException.GetType().Name,
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

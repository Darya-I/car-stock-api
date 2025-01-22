using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;


namespace CarStockAPI.Middlewares
{
    public abstract class AbstractExcepton
    {
        /// <summary>
        /// Делегат передачи управления  следующему middleware
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger<AbstractExcepton> _logger;

        /// <summary>
        /// Ловит исключение из бизнес слоя
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <returns>HTTP статус код и сообщение об ошибке</returns>
        public abstract (HttpStatusCode code, object response) GetException(Exception exception);

        public AbstractExcepton(RequestDelegate next, ILogger<AbstractExcepton> logger)
        {
            _next = next;
            _logger = logger;
        }


        /// <summary>
        /// Основная точка входа middleware.
        /// Обрабатывает входящие HTTP-запросы и перехватывает исключения для создания стандартного ответа.
        /// </summary>
        /// <param name="httpContext">Текущий контекст запроса</param>
        /// <returns>Задача, представляющая асинхронную операцию обработки запроса</returns>
        /// <exception cref="Exception">
        /// Любое необработанное исключение, возникающее во время выполнения запроса.
        /// </exception>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                var (status, message) = GetException(ex);
                response.StatusCode = (int)status;

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                };

                if (status == HttpStatusCode.InternalServerError)
                {
                    _logger.LogError("Error during executing {httpContext}. Details: {details}", httpContext.Request, ex.Message);
                }

                await response.WriteAsync(JsonSerializer.Serialize(message, options));
            }
        }
    }
}
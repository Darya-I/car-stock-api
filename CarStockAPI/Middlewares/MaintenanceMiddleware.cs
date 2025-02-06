using System.Linq;
using System.Text;
using CarStockDAL.Data.Interfaces.WS;

namespace CarStockAPI.Middlewares
{
    /// <summary>
    /// Middleware проверки тех. работ
    /// </summary>
    public class MaintenanceMiddleware
    {
        /// <summary>
        /// Делегат передачи управления следующему middleware
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Список допустимых путей, которые не проверяет middleware
        /// </summary>
        private readonly List<string> _excludedPaths = new List<string>
        {
            "/notifier.html",
            "/api/notifier/status",
            "/api/notifier/ws"
        };

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<MaintenanceMiddleware> _logger;

        public MaintenanceMiddleware(RequestDelegate next, 
            ILogger<MaintenanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Основная точка входа middleware.
        /// Обрабатывает HTTP-запросы и проверяет, есть ли тех. работы на сервере
        /// </summary>
        /// <param name="context">Текущий контекст запроса</param>
        /// <param name="repo">Репозиторий тех. работ</param>
        /// <returns><c>503</c> Если на сервере идут тех. работы</returns>
        public async Task InvokeAsync(HttpContext context, IMaintenanceRepository repo)
        {
            _logger.LogInformation($"Request: {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}");

            if (_excludedPaths.Contains(context.Request.Path))
            {
                _logger.LogInformation($"MaintenanceMiddleware: Request to {context.Request.Path} is allowed.");
                await _next(context);
                return;
            }
            if (await repo.IsMaintenanceActiveAsync())
            {
                _logger.LogInformation("MaintenanceMiddleware: Server is in maintenance mode.");
                context.Response.StatusCode = 503;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Сервер временно недоступен", Encoding.UTF8);
                return;
            }

            await _next(context);
        }
    }
}
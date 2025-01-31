using CarStockAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarStockAPI.Filters
{
    /// <summary>
    /// Фильтр действия проверки headers
    /// </summary>
    public class RequireAcceptHeaderFilter : IActionFilter
    {
        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<RequireAcceptHeaderFilter> _logger;

        /// <summary>
        /// Инициализирует экземляр фильтра действия проверки
        /// </summary>
        /// <param name="logger">Логгер</param>
        public RequireAcceptHeaderFilter(ILogger<RequireAcceptHeaderFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Проверяет есть в контексте заголовок Accept до выполнения действия
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("RequireAcceptHeaderFilter triggered!");

            var acceptHeader = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();
            if (string.IsNullOrEmpty(acceptHeader))
            {
                context.Result = new BadRequestObjectResult("Accept header is required");
            }
        }

        /// <summary>
        /// Просто чилловый парень
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

    }
}
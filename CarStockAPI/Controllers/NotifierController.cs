using CarStockBLL.Services.WS_Services;
using CarStockDAL.Data.Interfaces.WS;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    /// <summary>
    /// Контроллер для создание WebSocket соединения и отправки пуш-уведомлений
    /// </summary>
    [Route("api/notifier")]
    [ApiController]
    public class NotifierController : ControllerBase
    {
        /// <summary>
        /// Экземпляр обработчика WS соединений
        /// </summary>
        private readonly WebSocketHandler _webSocketHandler;

        /// <summary>
        /// Экземпляр репозитория тех. работ
        /// </summary>
        private readonly IMaintenanceRepository _maintenanceRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<NotifierController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера WS соединений
        /// </summary>
        /// <param name="webSocketHandler">Обработчик соединений</param>
        /// <param name="maintenanceRepository">Репозиторий тех. работ</param>
        /// <param name="logger">Логгер</param>
        public NotifierController(WebSocketHandler webSocketHandler, 
                                  IMaintenanceRepository maintenanceRepository,
                                  ILogger<NotifierController> logger)
        {
            _webSocketHandler = webSocketHandler;
            _maintenanceRepository = maintenanceRepository;
            _logger = logger;
        }

        /// <summary>
        /// Получает состояние тех. работ
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetMaintenanceStatus()
        {
            _logger.LogInformation("Atempting to recieve maintenance status");
            var isActive = await _maintenanceRepository.IsMaintenanceActiveAsync();
            _logger.LogInformation($"Status active {isActive}");
            return Ok(new { isActive });
        }

        /// <summary>
        /// Создает WS соединение
        /// </summary>
        [HttpGet("ws")]
        public async Task HandleWebSocketConnection()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _logger.LogInformation("Atempting to create web socket connection");
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _logger.LogInformation($"Web socket connection is active");
                await _webSocketHandler.HandleConnection(webSocket);
            }
            else
            {
                _logger.LogWarning("Cannot create web socket connection");
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
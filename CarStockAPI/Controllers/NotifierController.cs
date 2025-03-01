﻿using CarStockBLL.Services.WS_Services;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    /// <summary>
    /// Контроллер для создание WebSocket соединения
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
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<NotifierController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера WS соединений
        /// </summary>
        /// <param name="webSocketHandler">Обработчик соединений</param>
        /// <param name="logger">Логгер</param>
        public NotifierController(WebSocketHandler webSocketHandler, 
                                  ILogger<NotifierController> logger)
        {
            _webSocketHandler = webSocketHandler;
            _logger = logger;
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
using System.Net.WebSockets;
using System.Text;

namespace CarStockBLL.Services.WS_Services
{
    /// <summary>
    /// Класс для хранения и обработки активных WebSoket соединений
    /// </summary>
    public class WebSocketHandler
    {
        /// <summary>
        /// Список для хранения всех активных WS-соединений
        /// </summary>
        private readonly List<WebSocket> _sockets = new List<WebSocket>();

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<WebSocketHandler> _logger;

        public WebSocketHandler(ILogger<WebSocketHandler> logger) 
        {
            _logger = logger; 
        }

        /// <summary>
        /// Обрабатывает входящее WS-соединение 
        /// </summary>
        /// <param name="webSocket">Соединение WS</param>
        public async Task HandleConnection(WebSocket webSocket)
        {
            // Доступ к списку должен быть потокобезопасным (race condition)
            lock(_sockets) 
            {
                _sockets.Add(webSocket);
            }

            // Буффер для приема данных
            var buffer = new byte[1024 * 4];

            try
            {
                // Пока соединение открыто..
                while (webSocket.State == WebSocketState.Open)
                {
                    // ..Сервер ожидает получение данных
                    var result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        CancellationToken.None);
                    // Если не придет запрос на закрытие
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                }
            }
            finally
            {
                lock (_sockets)
                {
                    // Соединение удаляется из списка активных
                    _sockets.Remove(webSocket);
                }
                // Соединение закрывается
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                           "Closed by server",
                                           CancellationToken.None);
            }

        }

        /// <summary>
        /// Отправляет сообщение всем активным WS-соединениям
        /// </summary>
        /// <param name="message">Сообщение</param>
        public async Task BroadcastMessage(string message)
        {
            try
            {
                // Преобразование входящей строки в массив байтов в кодировке
                var buffer = Encoding.UTF8.GetBytes(message);

                // Скопировать все соединения, чтобы отправить сообщение без блокировки
                List<WebSocket> socketsCopy = new List<WebSocket>();

                lock (_sockets)
                {
                    socketsCopy = _sockets.ToList();
                }

                // Список задач на отправку
                var tasks = new List<Task>();

                // Перебор всех активных соединений
                foreach (var socket in socketsCopy)
                {
                    // Для каждого открытого соединения создается задача на отправку сообщения
                    if (socket.State == WebSocketState.Open)
                    {
                        tasks.Add(socket.SendAsync(new ArraySegment<byte>(buffer),
                                                   WebSocketMessageType.Text,
                                                   true,
                                                   CancellationToken.None));
                    }
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while broadcasting message on web socket. Details: {ex.Message}");
                throw;
            }
        }
    }
}
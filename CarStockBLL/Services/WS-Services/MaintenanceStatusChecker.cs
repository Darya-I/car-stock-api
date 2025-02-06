using CarStockDAL.Data.Interfaces.WS;

namespace CarStockBLL.Services.WS_Services
{
    /// <summary>
    /// Класс фоновой задачи, которая проверяет состояние тех. работ в БД и оповещения по WS
    /// </summary>
    public class MaintenanceStatusChecker : IHostedService, IDisposable
    {
        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<MaintenanceStatusChecker> _logger;

        /// <summary>
        /// Провайдер сервисов для получения зависимостей
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Обработчик WS-соединений
        /// </summary>
        private readonly WebSocketHandler _wsHandler; 
        
        /// <summary>
        /// Экземпляр таймера
        /// </summary>
        private Timer _timer;

        public MaintenanceStatusChecker(ILogger<MaintenanceStatusChecker> logger,
                                        WebSocketHandler webSocketHandler,
                                        IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _wsHandler = webSocketHandler;
            
        }

        /// <summary>
        /// При старте программы запускает фоновую задачу.
        /// Устанавливает таймер на каждые 5 минут. Метод <c>CheckMaintenance</c> вызывается сразу
        /// </summary>
        /// <param name="cancellationToken">Токен завершеия</param>
        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer
                (async state => await CheckMaintenance(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        /// <summary>
        ///  Проверяет состояние тех. работ на сервере и отправляет соответствующее сообщение
        ///  активным клиентам WS
        /// </summary>
        private async Task CheckMaintenance()
        {
            try
            {
                _logger.LogInformation("Checking server status");

                // Создаем новый scope для разрешения scoped репозитория
                using (var scope = _serviceProvider.CreateScope())
                {
                    var maintenanceRepository = scope.ServiceProvider.GetRequiredService<IMaintenanceRepository>();
                    bool hasMaintenance = await maintenanceRepository.IsMaintenanceActiveAsync();

                    string message = hasMaintenance
                        ? "Сервер недоступен, идут технические работы"
                        : "Сервер доступен";

                    await _wsHandler.BroadcastMessage(message);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while checking server status. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// При завершении программы завершает фоновую задачу.
        /// Останавливает таймер
        /// </summary>
        /// <param name="cancellationToken">Токен завершеия</param>
        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        } 

        /// <summary>
        /// Освобождает ресурсы таймера
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
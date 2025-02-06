using CarStockBLL.Hubs;
using CarStockDAL.Data.Interfaces.MaintenanceRepo;
using Microsoft.AspNetCore.SignalR;

namespace CarStockBLL.Services.SignalR_Services
{
    /// <summary>
    /// Класс фоновой задачи, которая проверяет состояние тех. работ в БД и оповещения по SignalR
    /// </summary>
    public class SrMaintenanceStatusChecker : IHostedService, IDisposable
    {
        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<SrMaintenanceStatusChecker> _logger;

        /// <summary>
        /// Провайдер сервисов для получения зависимостей
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Объект хаба для взаимодействия с подключенными клиентами
        /// </summary>
        private readonly IHubContext<NotifierHub> _hubContext;
        
        /// <summary>
        /// Таймер
        /// </summary>
        private Timer? _timer;

        public SrMaintenanceStatusChecker(ILogger<SrMaintenanceStatusChecker> logger,
                                          IServiceProvider serviceProvider,
                                          IHubContext<NotifierHub> hubContext)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        /// <summary>
        /// При старте программы запускает фоновую задачу.
        /// Устанавливает таймер на каждые 5 минут. Метод <c>CheckMaintenance</c> вызывается сразу
        /// </summary>
        /// <param name="cancellationToken">Токен завершеия</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                async state => await CheckMaintenance(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        /// <summary>
        ///  Проверяет состояние тех. работ на сервере и отправляет соответствующее сообщение
        ///  активным клиентам через <c>_hubContext</c>
        /// </summary>
        public async Task CheckMaintenance()
        {
            try
            {
                _logger.LogInformation("Checking server status");

                // Создаем новый scope для разрешения scoped репозитория
                using var scope = _serviceProvider.CreateScope();
                var maintenanceRepository = scope.ServiceProvider.GetRequiredService<IMaintenanceRepository>();

                bool hasMaintenance = await maintenanceRepository.IsMaintenanceActiveAsync();

                string message = hasMaintenance
                    ? "Сервер недоступен, идут технические работы"
                    : "Сервер доступен";

                await _hubContext.Clients.All.SendAsync("RecieveNotification", message);
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
        public Task StopAsync(CancellationToken cancellationToken)
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

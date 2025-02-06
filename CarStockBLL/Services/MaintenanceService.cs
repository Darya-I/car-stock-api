using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces.WS;
using CarStockDAL.Models.WS;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис для операций с тех. работами
    /// </summary>
    public class MaintenanceService : IMaitenanceService
    {
        /// <summary>
        /// Экземпляр репозитория для операций с тех. работами
        /// </summary>
        private readonly IMaintenanceRepository _maintenanceRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<MaintenanceService> _logger;

        public MaintenanceService(IMaintenanceRepository maintenanceRepository, ILogger<MaintenanceService> logger)
        {
            _maintenanceRepository = maintenanceRepository;
            _logger = logger;
        }
        /// <summary>
        /// Создает новую запись о тех. работах на час в базе данных
        /// </summary>
        public async Task CreateMaintenanceAsync()
        {
            try
            {
                var maintenance = new Maintenance
                {
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddHours(1)

                };

                await _maintenanceRepository.CreateMaintenanceAsync(maintenance.StartTime, maintenance.EndTime);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating maintenance. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Удаляет запись о тех. работах из базы данных
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteMaintenanceByIdAsync(int id)
        {
            try
            {
                await _maintenanceRepository.DeleteMaintenanceByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting maintenance. Details: {ex.Message}");
                throw;
            }
        }
    }
}

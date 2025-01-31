using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис операций над моделями автомобиля
    /// </summary>
    public class CarModelService : ICarModelService
    {
        /// <summary>
        /// Экземпляр репозитория для работы с моделями
        /// </summary>
        private ICarModelRepository<CarModel> _carModelRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<CarModel> _logger;

        /// Инициализирует новый экземпляр сервиса операций над моделями
        /// <param name="carModelRepository">Репозиторий для доступа к моделям автомобилей</param>
        /// <param name="logger">Логгер</param>
        public CarModelService(ICarModelRepository<CarModel> carModelRepository, ILogger<CarModel> logger)
        {
            _carModelRepository = carModelRepository;
            _logger = logger;
        }

        /// <summary>
        /// Получает модель по названию из базы данных
        /// </summary>
        /// <param name="name">Название модели</param>
        /// <returns>Модель</returns>
        public async Task<CarModel> GetCarModelByNameAsync(string? name)
        {
            _logger.LogInformation($"Fetching model with name {name}.");

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Failed to retrieve a car model. The provided model name was null or invalid.");
                throw new ValidationErrorException("Model name cannot be null or empty.");
            }

            var carModel = await _carModelRepository.GetCarModelByNameAsync(name);
            if (carModel == null)
            {
                _logger.LogWarning($"Car model with name {name} not found.");
                throw new EntityNotFoundException($"Car model with name '{name}' not found.");
            }

            return (new CarModel { Name = carModel.Name, Id = carModel.Id });
        }

        /// <summary>
        /// Получает модель автомобиля по идентификатору из базы данных
        /// </summary>
        /// <param name="id">Идентификатор модели</param>
        /// <returns>Объект модели автомобиля</returns>
        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching model with Id {id}.");

            var carModel = await _carModelRepository.GetCarModelByIdAsync(id);
            if (carModel == null)
            {
                _logger.LogWarning($"Car model with Id {id} not found.");
                throw new EntityNotFoundException($"Car model with Id '{id}' not found.");
            }
            return carModel;
        }
    }
}
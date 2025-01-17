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

        /// Инициализирует новый экземпляр сервиса операций над моделями
        /// <param name="carModelRepository">Репозиторий для доступа к моделям автомобилей</param>
        public CarModelService(ICarModelRepository<CarModel> carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }

        /// <summary>
        /// Получает модель по названию из базы данных
        /// </summary>
        /// <param name="name">Название модели</param>
        /// <returns>Модель</returns>
        public async Task<CarModel> GetCarModelByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Car model name cannot be null or empty.");
            }

            var carModel = await _carModelRepository.GetCarModelByNameAsync(name);
            if (carModel == null)
            {
                throw new KeyNotFoundException($"Car model with name '{name}' not found.");
            }

            return (new CarModel { Name = carModel.Name, Id = carModel.Id });
        }
    }
}

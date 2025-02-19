using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис операций над маркой автомобиля
    /// </summary>
    public class BrandService : IBrandService
    {
        /// <summary>
        /// Экземпляр репозитория для работы с марками автомобиля
        /// </summary>
        private readonly IBrandRepository<Brand> _brandRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<BrandService> _logger;

        /// Инициализирует новый экземпляр сервиса операций с марками
        /// <param name="brandRepository">Репозиторий для доступа к маркам автомобилей</param>
        /// <param name="logger">Логгер</param>
        public BrandService(IBrandRepository<Brand> brandRepository, ILogger<BrandService> logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }
        /// <summary>
        /// Получает марку автомобиля по названию из базы данных
        /// </summary>
        /// <param name="name">Название марки</param>
        /// <returns>Марка автомобиля</returns>
        public async Task<Brand> GetBrandByNameAsync(string? name)
        {
            _logger.LogInformation($"Fetching brand with name {name}.");

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Failed to retrieve a brand. The provided brand name was null or invalid.");
                throw new ValidationErrorException("Brand name cannot be null or empty.");
            }

            var brand = await _brandRepository.GetBrandByNameAsync(name);

            if (brand == null)
            {
                _logger.LogWarning($"Brand with name {name} not found.");
                throw new EntityNotFoundException($"Brand with name '{name}' not found.");
            }

            return (new Brand { Name = brand.Name, Id = brand.Id });
        }

        /// <summary>
        /// Получает марку автомобиля по идентификатору из базы данных
        /// </summary>
        /// <param name="id">Идентификатор марки</param>
        /// <returns>Объект марки автомобиля</returns>
        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching brand with Id {id}.");

            var brand = await _brandRepository.GetBrandByIdAsync(id);

            if (brand == null)
            {
                _logger.LogWarning($"Brand with Id {id} not found.");
                throw new EntityNotFoundException($"Brand with Id '{id}' not found.");
            }

            return brand;
        }

        public Task<List<Brand>> GetAllBrandsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
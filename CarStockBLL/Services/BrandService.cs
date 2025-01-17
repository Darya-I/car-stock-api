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
        
        /// Инициализирует новый экземпляр сервиса операций с марками
        /// <param name="brandRepository">Репозиторий для доступа к маркам автомобилей</param>
        public BrandService(IBrandRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        /// <summary>
        /// Получает марку автомобиля по названию из базы данных
        /// </summary>
        /// <param name="name">Название марки</param>
        /// <returns>Марка автомобиля</returns>
        public async Task<Brand> GetBrandByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Brand name cannot be null or empty.");
            }

            var brand = await _brandRepository.GetBrandByNameAsync(name);

            if (brand == null)
            {
                throw new KeyNotFoundException($"Brand with name '{name}' not found.");
            }

            return (new Brand { Name = brand.Name, Id = brand.Id });
        }
    }
}
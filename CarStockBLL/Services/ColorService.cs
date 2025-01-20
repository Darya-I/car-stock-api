using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис операций над цветом автомобиля
    /// </summary>
    public class ColorService : IColorService
    {
        /// <summary>
        /// Экземпляр репозитория для работы с цветом автомобиля
        /// </summary>
        private readonly IColorRepository<Color> _colorRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<Color> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над цветом
        /// </summary>
        /// <param name="colorRepository">Репозиторий для доступа к цветам автомобилей</param>
        /// <param name="logger">Логгер</param>
        public ColorService(IColorRepository<Color> colorRepository, ILogger<Color> logger)
        {
            _colorRepository = colorRepository;
            _logger = logger;
        }

        /// <summary>
        /// Получает цвет по названию из базы данных
        /// </summary>
        /// <param name="name">Название цвета</param>
        /// <returns>Цвет</returns>
        public async Task<Color> GetColorByNameAsync(string? name)
        {
            _logger.LogInformation("Fetching color with name {name}.", name);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Attempted to retrieve a color with a null name.");
                throw new ValidationErrorException("Color name cannot be null or empty.");
            }

            var color = await _colorRepository.GetColorByNameAsync(name);
            
            if (color == null)
            {
                _logger.LogWarning("Color with name {name} not found.", name);
                throw new EntityNotFoundException($"Color with name '{name}' not found.");
            }

            return (new Color { Name = color.Name, Id = color.Id });
        }
    }
}
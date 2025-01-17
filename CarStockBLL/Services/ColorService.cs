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
        /// Инициализирует новый экземпляр сервиса операций над цветом
        /// </summary>
        /// <param name="colorRepository">Репозиторий для доступа к цветам автомобилей</param>
        public ColorService(IColorRepository<Color> colorRepository)
        {
            _colorRepository = colorRepository;
        }

        /// <summary>
        /// Получает цвет по названию из базы данных
        /// </summary>
        /// <param name="name">Название цвета</param>
        /// <returns>Цвет</returns>
        public async Task<Color> GetColorByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Color name cannot be null or empty.");
            }

            var color = await _colorRepository.GetColorByNameAsync(name);
            
            if (color == null)
            {
                throw new KeyNotFoundException($"Color with name '{name}' not found.");
            }

            return (new Color { Name = color.Name, Id = color.Id });
        }
    }
}

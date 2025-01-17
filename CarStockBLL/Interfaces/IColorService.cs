using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс операций над цветом автомобиля
    /// </summary>
    public interface IColorService
    {
        /// <summary>
        /// Получает цвет по названию из базы данных
        /// </summary>
        /// <param name="name">Название цвета</param>
        /// <returns>Цвет</returns>
        Task<Color> GetColorByNameAsync(string? name);   
    }
}

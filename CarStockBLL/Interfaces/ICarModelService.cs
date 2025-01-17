using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс операций над моделью автомобиля
    /// </summary>
    public interface ICarModelService
    {
        /// <summary>
        /// Получает модель по названию из базы данных
        /// </summary>
        /// <param name="name">Название модели</param>
        /// <returns>Модель</returns>
        Task<CarModel> GetCarModelByNameAsync(string? name);
    }
}

using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса операций над маркой автомобиля
    /// </summary>
    public interface IBrandService
    {
        /// <summary>
        /// Получает марку автомобиля по названию из базы данных
        /// </summary>
        /// <param name="name">Название марки</param>
        /// <returns>Марка автомобиля</returns>
        Task<Brand> GetBrandByNameAsync(string? name);
    }
}

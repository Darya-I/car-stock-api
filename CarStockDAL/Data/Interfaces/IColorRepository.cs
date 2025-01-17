using CarStockDAL.Models;


namespace CarStockDAL.Data.Interfaces
{
    /// <summary>
    /// Интерфейс для операций над данными цвета автомобилей
    /// </summary>
    /// <typeparam name="T">Тип сущности для работы с цветом автомобилей</typeparam>
    public interface IColorRepository<T>
        where T : class
    {
        /// <summary>
        /// Создает новую запись цвета автомобиля
        /// </summary>
        /// <param name="color">Объект цвета автомобиля для создания</param>
        Task CreateColorAsync(T color);

        /// <summary>
        /// Обновляет запись цвета автомобиля
        /// </summary>
        /// <param name="color">Объект цвета автомобиля для обновления</param>
        Task UpdateColorAsync(T color);

        /// <summary>
        /// Удаляет запись цвета автомобиля по Id
        /// </summary>
        /// <param name="id">Id цвета</param>
        Task DeleteColorAsync(int id);

        /// <summary>
        /// Получает запись цвета автомобиля по Id
        /// </summary>
        /// <param name="id">Id цвета</param>
        /// <returns>Цвет автомобиля</returns>
        Task<T> GetColorByIdAsync(int id);

        /// <summary>
        /// Получает список всех цветов автомобилей
        /// </summary>
        /// <param name="tracked">Указывает, отслеживаются ли изменения сущностей. По умолчанию true</param>
        /// <returns>Список цветов</returns>
        Task<List<Color>> GetAllColorsAsync(bool tracked = true);

        /// <summary>
        /// Получает цвет автомобиля по названию
        /// </summary>
        /// <param name="name">Название цвета</param>
        /// <returns>Цвет автомобиля</returns>
        Task<T> GetColorByNameAsync(string name);
    }
}

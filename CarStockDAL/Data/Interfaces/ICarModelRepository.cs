using CarStockDAL.Models;

namespace CarStockDAL.Data.Interfaces
{
    /// <summary>
    /// Интерфейс для операций над данными моделей автомобилей
    /// </summary>
    /// <typeparam name="T">Тип сущности для работы с моделями автомобилей</typeparam>
    public interface ICarModelRepository<T>
        where T : class
    {
        /// <summary>
        /// Создает новую запись модели автомобиля
        /// </summary>
        /// <param name="carModel">Объект модели автомобиля для создания</param>
        Task CreateCarModelAsync(T carModel);

        /// <summary>
        /// Обновляет запись модели автомобиля
        /// </summary>
        /// <param name="carModel">Объект модели автомобиля для обновления</param>
        Task UpdateCarModelAsync(T carModel);

        /// <summary>
        /// Удаляет запись модели автомобиля по Id
        /// </summary>
        /// <param name="id">Id модели</param>
        Task DeleteCarModelAsync(int id);

        /// <summary>
        /// Получает запись модели автомобиля по Id
        /// </summary>
        /// <param name="id">Id модели</param>
        /// <returns>Марка автомобиля</returns>
        Task<T> GetCarModelByIdAsync(int id);

        /// <summary>
        /// Получает список всех моделей автомобилей
        /// </summary>
        /// <param name="tracked">Указывает, отслеживаются ли изменения сущностей. По умолчанию true</param>
        /// <returns>Список моделей</returns>
        Task<List<CarModel>> GetCarModelsAsync(bool tracked = true);

        /// <summary>
        /// Получает модель автомобиля по названию
        /// </summary>
        /// <param name="name">Название модели</param>
        /// <returns>Модель автомобиля</returns>
        Task<T> GetCarModelByNameAsync(string name);
    }
}

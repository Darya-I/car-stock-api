using CarStockDAL.Models;

namespace CarStockDAL.Data.Interfaces
{
    /// <summary>
    ///  Интерфейс для операций над данными марок автомобилей
    /// </summary>
    /// <typeparam name="T">Тип сущности для работы с марками автомобилей</typeparam>
    public interface IBrandRepository<T>
        where T : class
    {
        /// <summary>
        /// Создает новую запись марки автомобиля
        /// </summary>
        /// <param name="brand">Объект марки автомобиля для создания</param>
        Task CreareBrandAsync(T brand);

        /// <summary>
        /// Обновляет запись марки автомобиля
        /// </summary>
        /// <param name="brand">Объект марки автомобиля для обновления</param>
        Task UpdateBrandAsync(T brand);

        /// <summary>
        /// Удаляет запись марки автомобиля по Id
        /// </summary>
        /// <param name="id">Id марки</param>
        Task DeleteBrandAsync(int id);

        /// <summary>
        /// Получает запись марки автомобиля по Id
        /// </summary>
        /// <param name="id">Id марки</param>
        /// <returns>Марка автомобиля</returns>
        Task<T> GetBrandByIdAsync(int id);

        /// <summary>
        /// Получает список всех марок автомобилей
        /// </summary>
        /// <param name="tracked">Указывает, отслеживаются ли изменения сущностей. По умолчанию true</param>
        /// <returns>Список марок</returns>
        Task<List<Brand>> GetAllBrandsAsync(bool tracked = true);

        /// <summary>
        /// Получает марку автомобиля по названию
        /// </summary>
        /// <param name="name">Название марки</param>
        /// <returns>Марка автомобиля</returns>
        Task<T> GetBrandByNameAsync(string name);

    }
}

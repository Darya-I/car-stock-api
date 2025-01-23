using CarStockDAL.Models;

namespace CarStockDAL.Data.Interfaces
{
    /// <summary>
    /// Интерфейс для операций над данными автомобилей
    /// </summary>
    /// <typeparam name="T">Тип сущности для работы с автомобилями</typeparam>
    public interface ICarRepository<T>
            where T : class
    {
        /// <summary>
        /// Создает новую запись автомобиля
        /// </summary>
        /// <param name="car">Объект автомобиля для создания</param>
        Task CreateCarAsync(T car);

        /// <summary>
        /// Обновляет запись автомобиля
        /// </summary>
        /// <param name="car">Объект автомобиля для обновления</param>
        Task UpdateCarAsync(T car);

        /// <summary>
        /// Удаляет запись автомобиля по Id
        /// </summary>
        /// <param name="id">Id автомобиля</param>
        Task DeleteCarAsync(int id);

        /// <summary>
        /// Получает запись автомобиля по Id
        /// </summary>
        /// <param name="id">Id автомобиля</param>
        /// <returns>Автомобиль</returns>
        Task<T> GetCarByIdAsync(int id);

        /// <summary>
        /// Получает список всех автомобилей
        /// </summary>
        /// <param name="tracked">Указывает, отслеживаются ли изменения сущностей. По умолчанию true</param>
        /// <returns>Список автомобилей</returns>
        Task<List<Car>> GetAllCarsAsync(bool tracked = true);

        /// <summary>
        /// Вспомогательный метод для проверки наличия автомобиля
        /// </summary>
        /// <param name="brandId">Идентификатор марки</param>
        /// <param name="carModelId">Идентификатор модели</param>
        /// <param name="colorId">Идентификатор цвета</param>
        /// <returns><c>true</c> если машина создана, иначе <c>false</c></returns>
        Task<bool> CarExistAsync(int brandId, int carModelId, int colorId);
    }
}

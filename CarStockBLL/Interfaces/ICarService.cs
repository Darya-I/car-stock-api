using CarStockDAL.Models;


namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса операций над автомобилями
    /// </summary>
    public interface ICarService
    {
        /// <summary>
        /// Получает автомобиль из базы данных
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>Автомобиль</returns>
        Task<Car> GetCarByIdAsync(int? id);

        /// <summary>
        /// Получает список автомобилей из базы данных
        /// </summary>
        /// <returns>Коллекция автомобилей</returns>
        Task<IEnumerable<Car>> GetAllCarsAsync();

        /// <summary>
        /// Создает новый автомобиль в базе данных
        /// </summary>
        /// <param name="car">Автомобиль</param>
        /// <returns>Информация о новом автомобиле</returns>
        Task<string> CreateCarAsync(Car car);

        /// <summary>
        /// Обновляет информацию об автомобиле в базе данных
        /// </summary>
        /// <param name="car">Автомобиль</param>
        /// <returns>Значение <c>true</c>, если обновление выполнено успешно; иначе <c>false</c>.</returns>
        Task<Car> UpdateCarAsync(Car car);

        /// <summary>
        /// Удаляет автомобиль из базы данных
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        Task DeleteCarAsync(int? id);

        /// <summary>
        /// Обновляет доступность автомобиля
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="isAvaible">Доступность</param>
        Task UpdateCarAvailabilityAsync(int id, bool isAvaible);

        /// <summary>
        /// Обновляет количество автомобилей
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="amount">Количество</param>
        Task UpdateCarAmountAsync(int id, int amount);
    }
}

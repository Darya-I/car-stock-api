using CarStockBLL.DTO;
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
        /// <returns>DTO автомобиля</returns>
        Task<GetCarDTO> GetCarByIdAsync(int id);

        /// <summary>
        /// Получает список автомобилей из базы данных
        /// </summary>
        /// <returns>Коллекция DTO автомобилей</returns>
        Task<List<GetCarDTO>> GetAllCarsAsync();

        /// <summary>
        /// Создает новый автомобиль в базе данных
        /// </summary>
        /// <param name="car">Автомобиль</param>
        /// <returns>DTO информация о новом автомобиле</returns>
        Task<CarDTO> CreateCarAsync(Car car);

        /// <summary>
        /// Обновляет информацию об автомобиле в базе данных
        /// </summary>
        /// <param name="car">Автомобиль</param>
        /// <returns>DTO обновленного автомобиля</returns>
        Task<CarDTO> UpdateCarAsync(Car car);

        /// <summary>
        /// Удаляет автомобиль из базы данных
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        Task DeleteCarAsync(int id);

        /// <summary>
        /// Обновляет доступность автомобиля
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="isAvaible">Доступность</param>
        /// <returns>DTO доступности автомобиля</returns>
        Task<CarAvailabilityDTO> UpdateCarAvailabilityAsync(int id, bool isAvaible);

        /// <summary>
        /// Обновляет количество автомобилей
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="amount">Количество</param>
        /// <returns>DTO количества автомобиля</returns>
        Task<CarAmountDTO> UpdateCarAmountAsync(int id, int amount);

    }
}
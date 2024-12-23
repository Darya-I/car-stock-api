using CarStockDAL.Models;

namespace CarStockDAL.Data.Repos
{
    /// <summary>
    /// На случай, если контекст данных будет подразумевать освобождение или закрытие подключений, интерфейс репозитория применяет интерфейс IDisposable.
    /// </summary>
    public interface ICarRepository<T>
            where T : class
    {

        Task CreateCarAsync(T car);
        Task UpdateCarAsync(T car);
        Task DeleteCarAsync(int id);
        Task<T> GetCarByIdAsync(int id);
        Task<List<Car>> GetAllCarsAsync(bool tracked = true);
        Task UpdateCarAvailabilityAsync(int id, bool isAvaible);

        Task SaveAsync();

    }
}

using CarStockDAL.Models;

namespace CarStockDAL.Data
{
    /// <summary>
    /// На случай, если контекст данных будет подразумевать освобождение или закрытие подключений, интерфейс репозитория применяет интерфейс IDisposable.
    /// </summary>
    public interface ICarRepository<T> : IDisposable
            where T : class
    {
        IEnumerable<T> GetAllCars();
        T GetCar(int id);
        void Create(T car);
        void Update(T car);
        void Delete(int id);
        void Save();
    }
}

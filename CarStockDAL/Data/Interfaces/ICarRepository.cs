using CarStockDAL.Models;

namespace CarStockDAL.Data.Interfaces
{
    public interface ICarRepository<T>
            where T : class
    {
        Task CreateCarAsync(T car);
        Task UpdateCarAsync(T car);
        Task DeleteCarAsync(int id);
        Task<T> GetCarByIdAsync(int id);
        Task<List<Car>> GetAllCarsAsync(bool tracked = true);
    }
}

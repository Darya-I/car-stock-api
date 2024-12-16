using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface ICarService
    {
        // read
        // в этих методах хз Т или сам класс
        Task<Car> GetCarByIdAsync(int? id);
        Task<IEnumerable<Car>> GetAllCarsAsync();

        Task CreateCarAsync(Car car);

        Task UpdateCarAsync(Car car);

        Task DeleteCarAsync(int? id);

    }
}

using CarStockBLL.Models;
using CarStockDAL.Models;


namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса операций над автомобилями
    /// </summary>
    public interface ICarService
    {
        Task<Car> GetCarByIdAsync(int? id);
        Task<IEnumerable<Car>> GetAllCarsAsync();

        Task<OperationResult<string>> CreateCarAsync(Car car);

        Task<bool> UpdateCarAsync(Car car);

        Task DeleteCarAsync(int? id);

        Task UpdateCarAvailabilityAsync(int id, bool isAvaible);

        Task UpdateCarAmountAsync(int id, int amount);
    }
}

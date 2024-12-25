using CarStockBLL.Models;
using CarStockDAL.Models;


namespace CarStockBLL.Interfaces
{
    public interface ICarService
    {
        // read
        // в этих методах хз Т или сам класс
        Task<Car> GetCarByIdAsync(int? id);
        Task<IEnumerable<Car>> GetAllCarsAsync();

        Task<OperationResult<string>> CreateCarAsync(Car car);

        Task UpdateCarAsync(CarUpdateDto carUpdateDto);

        Task DeleteCarAsync(int? id);

        Task UpdateCarAvailabilityAsync(int id, bool isAvaible);

        Task UpdateCarAmountAsync(int id, int amount);

    }
}

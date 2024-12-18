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

        Task<OperationResult<string>> CreateCarAsync(string brandName, string carModelName, string colorName, int amount, bool isAvaible);

        Task UpdateCarAsync(CarUpdateDto carUpdateDto);

        Task DeleteCarAsync(int? id);

        //Task<bool> IsCarExists(string BrandName, string CarModelName, string ColorName);

    }
}

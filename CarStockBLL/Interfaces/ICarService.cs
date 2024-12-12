using CarStockBLL.DTO;
using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface ICarService
    {

        // не знаю делать их асинхронными или нет
        void CreateCar(Car car);

        // read
        Car GetCar(int? id);
        //read
        IEnumerable<Car> GetCars();

        void UpdateCar(Car car);

        void DeleteCar(int? id);

        void Dispose();


    }
}

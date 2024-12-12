using System.ComponentModel.DataAnnotations;
using CarStockBLL.DTO;
using CarStockBLL.Interfaces;
using CarStockDAL.Data;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository<Car> _carRepository;
        //private readonly IColorRepository<Color> _colorRepository;

        public CarService(ICarRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }


        public Car GetCar(int? id) 
        {
            if (id == null)
            {
                throw new ValidationException("");
            }

            var car = _carRepository.GetCar(id.Value);

            return new Car { Brand = car.Brand, CarModel = car.CarModel, Color = car.Color, Amount = car.Amount, IsAvaible = car.IsAvaible };
        }

        public void UpdateCar(Car car) 
        {
            var mod_car = _carRepository.GetCar(car.Id);
            
            if (mod_car == null)
            {
                throw new ValidationException("Car not found.");
            }

            car.Brand.Id = mod_car.BrandId;
            car.CarModel.Id = mod_car.CarModelId;
            car.Color = mod_car.Color;
            car.Amount = mod_car.Amount;
            car.IsAvaible = mod_car.IsAvaible;

            _carRepository.Update(mod_car);
            _carRepository.Save();
        }

        public void DeleteCar(int? id) 
        {
            if (id == null) 
            {
                throw new ValidationException(" "); //дописать тут
            }
            var car = _carRepository.GetCar(id.Value);

            if (car == null)
            {
                throw new ValidationException(" ");
            }

            _carRepository.Delete(id.Value);
            _carRepository.Save();

        }

        public IEnumerable<Car> GetCars() 
        {
            var cars = _carRepository.GetAllCars();

            if (cars == null || !cars.Any())
            {
                throw new ValidationException("Автомобили не найдены."); // Или используйте свое исключение
            }

            return cars.Select(car => new Car
            {
                Brand = car.Brand,
                CarModel = car.CarModel,
                Color = car.Color,
                Amount = car.Amount,
                IsAvaible= car.IsAvaible
            });

            
        }

        public void Dispose()
        {
            _carRepository.Dispose();

        }

        public void CreateCar(Car car)
        {
            _carRepository.Create(car);
            _carRepository.Save();

        }
    }
}

using System.ComponentModel.DataAnnotations;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Repos;
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


        public async Task<Car> GetCarByIdAsync(int? id) 
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Car ID cannot be null.");
            }

            var car = await _carRepository.GetCarByIdAsync(id.Value);

            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {id} not found.");
            }

            return new Car
            {
                Id = car.Id,
                Brand = car.Brand,
                CarModel = car.CarModel,
                Color = car.Color,
                Amount = car.Amount,
                IsAvaible = car.IsAvaible,
            };
        }

        public async Task UpdateCarAsync(Car car) 
        {
            var existingCar = await _carRepository.GetCarByIdAsync(car.Id);

            if (existingCar == null)
            {
                throw new ValidationException("Car not found.");
            }

            existingCar.Brand.Id = car.BrandId;
            existingCar.CarModel.Id = car.CarModelId;
            existingCar.Color = car.Color;
            existingCar.Amount = car.Amount;
            existingCar.IsAvaible = car.IsAvaible;

            await _carRepository.UpdateCarAsync(car);
            await _carRepository.SaveAsync();
        }

        public async Task DeleteCarAsync(int? id) 
        {
            if (id == null) 
            {
                throw new ValidationException(" "); //дописать тут
            }
            var car = _carRepository.GetCarByIdAsync(id.Value);

            if (car == null)
            {
                throw new ValidationException(" ");
            }

            await _carRepository.DeleteCarAsync(id.Value);
            await _carRepository.SaveAsync();

        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync() 
        {
            try
            {
                var cars = await _carRepository.GetAllCarsAsync();
                if (cars.Any())
                {
                    return cars;
                }
                else
                {
                    return Enumerable.Empty<Car>();

                }

            }
            catch (Exception ex)
            {
                throw new ValidationException("Car not found.");
            }

        }

        public async Task CreateCarAsync(Car car)
        {
            _carRepository.CreateCarAsync(car);
            _carRepository.SaveAsync();

        }
        
    }
}

using System.ComponentModel.DataAnnotations;
using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository<Car> _carRepository;

        private readonly IBrandService _brandService;
        private readonly ICarModelService _carModelService;
        private readonly IColorService _colorService;

        public CarService(ICarRepository<Car> carRepository, IBrandService brandService, ICarModelService carModelService, IColorService colorService)
        {
            _carRepository = carRepository;
            _brandService = brandService;
            _carModelService = carModelService;
            _colorService = colorService;
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

            // Возвращаем результат
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

        public async Task UpdateCarAsync(CarUpdateDto carUpdateDto)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(carUpdateDto.Id);

            if (existingCar == null)
            {
                throw new ValidationException("Car not found.");
            }

            existingCar.BrandId = carUpdateDto.BrandId;
            existingCar.CarModelId = carUpdateDto.CarModelId;
            existingCar.ColorId = carUpdateDto.ColorId;
            existingCar.Amount = carUpdateDto.Amount;
            existingCar.IsAvaible = carUpdateDto.IsAvaible;

            await _carRepository.UpdateCarAsync(existingCar);
        }

        public async Task DeleteCarAsync(int? id) 
        {
            if (id == null) 
            {
                throw new ValidationException("Car ID cannot be null");
            }
            var car = await _carRepository.GetCarByIdAsync(id.Value);

            if (car == null)
            {
                throw new ValidationException("Сar not found");
            }

            await _carRepository.DeleteCarAsync(id.Value);
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync() 
        {
            var cars = await _carRepository.GetAllCarsAsync();
            return cars.Any() ? cars : Enumerable.Empty<Car>();
        }
        //                                                      REFACTOR ????????
        public async Task<OperationResult<string>> CreateCarAsync(Car car)
        {
            var brandResult = await _brandService.GetBrandByNameAsync(car.Brand.Name);
            if (!brandResult.Success) 
            {
                return OperationResult<string>.Failure(brandResult.ErrorMessage);
            }
            var brand = brandResult.Data;

            var colorResult = await _colorService.GetColorByNameAsync(car.Color.Name);
            if (!colorResult.Success)
            {
                return OperationResult<string>.Failure(colorResult.ErrorMessage);
            }
            var color = colorResult.Data;

            var carModelResult = await _carModelService.GetCarModelByNameAsync(car.CarModel.Name);
            if (!carModelResult.Success)
            {
                return OperationResult<string>.Failure(carModelResult.ErrorMessage);
            }
            var carModel = carModelResult.Data;

            var newCar = new Car
            {
                BrandId = brand.Id,
                CarModelId = carModel.Id,
                ColorId = color.Id,
                Amount = car.Amount,
                IsAvaible = car.IsAvaible,
            };

            await _carRepository.CreateCarAsync(newCar);

            return OperationResult<string>.SuccessResult($"Car '{car.CarModel.Name}' of brand '{car.Brand.Name}' and color '{car.Color.Name}' successfully created.");
        }

        public async Task UpdateCarAvailabilityAsync(int id, bool isAvaible)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(id);

            if (existingCar == null)
            {
                throw new ValidationException("Car not found.");
            }

            existingCar.IsAvaible = isAvaible;

            await _carRepository.UpdateCarAsync(existingCar);
        }

        public async Task UpdateCarAmountAsync(int id, int amount)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(id);

            if (existingCar == null) 
            {
                throw new ValidationException("Car not found");
            }

            if (amount < 0)
            {
                throw new ValidationException("Amount cannot be negative.");
            }

            existingCar.Amount = amount;

            existingCar.IsAvaible = amount > 0;

            await _carRepository.UpdateCarAsync(existingCar);       
        }
    }
}

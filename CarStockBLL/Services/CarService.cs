using System.ComponentModel.DataAnnotations;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository<Car> _carRepository;

        private readonly IBrandService _brandService;
        private readonly ICarModelService _carModelService;
        private readonly IColorService _colorService;
        private readonly ILogger<CarService> _logger;

        public CarService(
            ICarRepository<Car> carRepository, 
            IBrandService brandService, 
            ICarModelService carModelService, 
            IColorService colorService,
            ILogger<CarService> logger)
        {
            _carRepository = carRepository;
            _brandService = brandService;
            _carModelService = carModelService;
            _colorService = colorService;
            _logger = logger;
        }

        public async Task<Car> GetCarByIdAsync(int? id)
        {
            _logger.LogInformation("Fetching car with ID {CarId}.", id);

            if (id == null)
            {
                _logger.LogWarning("Attempted to retrieve a car with a null ID.");
                throw new ArgumentNullException(nameof(id), "Car ID cannot be null.");
            }

            if (id < 1)
            {
                _logger.LogWarning("Attempted to retrieve a car with a negative ID.");
                throw new ValidationException("Id must be greater than 0");
            }

            try
            {
                var car = await _carRepository.GetCarByIdAsync(id.Value);

                if (car == null)
                {
                    _logger.LogWarning("Car with ID {CarId} not found.", id);
                    throw new KeyNotFoundException($"Car with ID {id} not found.");
                }

                _logger.LogInformation("Car with ID {CarId} successfully retrieved.", id);
                
                return new Car
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    CarModel = car.CarModel,
                    Color = car.Color,
                    Amount = car.Amount,
                    IsAvailable = car.IsAvailable,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the car with ID {CarId}.", id);
                throw;
            }
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            _logger.LogInformation("Fetching car with ID {CarId}.", car.Id);

            try
            {               
                var existingCar = await _carRepository.GetCarByIdAsync(car.Id);

                if (existingCar == null)
                {
                    _logger.LogWarning("Car with ID {CarId} not found.", car.Id);
                    throw new KeyNotFoundException("Car not found.");
                }

                existingCar.BrandId = car.BrandId;
                existingCar.CarModelId = car.CarModelId;
                existingCar.ColorId = car.ColorId;
                existingCar.Amount = car.Amount;
                existingCar.IsAvailable = car.IsAvailable;

                await _carRepository.UpdateCarAsync(existingCar);
                
                _logger.LogInformation("Car with ID {CarId} successfully updated.", car.Id);

                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while updating the car with ID {CarId}.", car.Id);
                throw;
            }
        }

        public async Task DeleteCarAsync(int? id) 
        {
            if (id == null) 
            {
                _logger.LogWarning("Attempted to retrieve a car with a null ID.");
                throw new ArgumentNullException(nameof(id), "Car ID cannot be null.");
            }

            try
            {
                var car = await _carRepository.GetCarByIdAsync(id.Value);

                if (car == null)
                {
                    _logger.LogWarning("Car with ID {CarId} not found.", id);
                    throw new KeyNotFoundException("Сar not found");
                }

                _logger.LogInformation("Car with ID {CarId} successfully deleted.", car.Id);

                await _carRepository.DeleteCarAsync(id.Value);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while deleting the car with ID {CarId}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync() 
        {
            try
            {
                var cars = await _carRepository.GetAllCarsAsync();
                return cars.Any() ? cars : Enumerable.Empty<Car>();
            }
            catch (Exception ex) 
            {
                _logger.LogError("An error occurred while retrieving cars. Details: {Details}", ex.Message);
                throw;
            }
        }

        public async Task<string> CreateCarAsync(Car car)
        {
            try
            {
                _logger.LogInformation("Creating car");

                var brand = await _brandService.GetBrandByNameAsync(car.Brand.Name);
                var carModel = await _carModelService.GetCarModelByNameAsync(car.CarModel.Name);
                var color = await _colorService.GetColorByNameAsync(car.Color.Name);

                var newCar = new Car
                {
                    BrandId = brand.Id,
                    CarModelId = carModel.Id,
                    ColorId = color.Id,
                    Amount = car.Amount,
                    IsAvailable = car.IsAvailable,
                };


                await _carRepository.CreateCarAsync(newCar);

                _logger.LogInformation("Car with ID {CarId} successfully created.", newCar.Id);

                return $"Car '{car.CarModel.Name}' of brand '{car.Brand.Name}' and color '{car.Color.Name}' successfully created.";
            }

            catch (ArgumentException ex) 
            {
                _logger.LogWarning(ex, "Invalid argument: {Details}", ex.Message);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Missing data: {Details}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating car. Details: {Details}", ex.Message);
                throw;
            }
        }

        public async Task UpdateCarAvailabilityAsync(int id, bool IsAvailable)
        {
            try
            {
                _logger.LogInformation("Fetching car with ID {CarId}.", id);

                var existingCar = await _carRepository.GetCarByIdAsync(id);

                existingCar.IsAvailable = IsAvailable;

                await _carRepository.UpdateCarAsync(existingCar);

            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex) 
            {
                _logger.LogError("An error occurred while updating availability of car with ID {CarId}. Details: {Details}", id, ex.Message);
                throw;
            }
        }

        public async Task UpdateCarAmountAsync(int id, int amount)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(id);

            try 
            {
                if (existingCar == null)
                {
                    throw new ValidationException("Car not found");
                }

                if (amount < 0)
                {
                    throw new ValidationException("Amount cannot be negative.");
                }

                existingCar.Amount = amount;

                existingCar.IsAvailable = amount > 0;

                await _carRepository.UpdateCarAsync(existingCar);
            }
            catch (ValidationException)
            {
                _logger.LogError("An error occurred while deleting the car with ID {CarId}.", id);
                throw;
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис операций над автомобилями
    /// </summary>
    public class CarService : ICarService
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        /// <summary>
        /// Экземпляр сервиса операций над марками
        /// </summary>
        private readonly IBrandService _brandService;

        /// <summary>
        /// Экземпляр сервиса операций над моделями
        /// </summary>
        private readonly ICarModelService _carModelService;

        /// <summary>
        /// Экземпляр сервиса операций над цветом
        /// </summary>
        private readonly IColorService _colorService;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<CarService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над автомобилями
        /// </summary>
        /// <param name="carRepository">Репозиторий доступа к автомобилям</param>
        /// <param name="brandService">Сервис операций над маркой автомобиля</param>
        /// <param name="carModelService">Сервис операций над моделями автомобиля</param>
        /// <param name="colorService">Сервис операций над цветом автомобиля</param>
        /// <param name="logger">Логгер</param>
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

        /// <summary>
        /// Получает автомобиль из базы данных
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>Автомобиль</returns>
        public async Task<Car> GetCarByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching car with ID {id}.");

            try
            {
                var car = await _carRepository.GetCarByIdAsync(id);

                if (car == null)
                {
                    _logger.LogWarning($"Car with ID {id} not found.");
                    throw new EntityNotFoundException($"Car with ID {id} not found.");
                }

                _logger.LogInformation($"Car with ID {id} successfully retrieved.");
                
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
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving the car with ID {id}. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Обновляет информацию об автомобиле в базе данных
        /// </summary>
        /// <param name="car">Автомобиль</param>
        /// <returns>Значение <c>true</c>, если обновление выполнено успешно; иначе <c>false</c>.</returns>
        public async Task<Car> UpdateCarAsync(Car car)
        {
            _logger.LogInformation($"Fetching car with ID {car.Id}.");

            try
            {               
                var existingCar = await _carRepository.GetCarByIdAsync(car.Id);

                if (existingCar == null)
                {
                    _logger.LogWarning($"Car with ID {car.Id} not found.");
                    throw new EntityNotFoundException($"Car with ID {car.Id} not found.");
                }

                existingCar.BrandId = car.BrandId;
                existingCar.CarModelId = car.CarModelId;
                existingCar.ColorId = car.ColorId;
                existingCar.Amount = car.Amount;
                existingCar.IsAvailable = car.IsAvailable;

                await _carRepository.UpdateCarAsync(existingCar);
                
                _logger.LogInformation($"Car with ID {car.Id} successfully updated.");

                return car;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while updating the car with ID {car.Id}. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Удаляет автомобиль из базы данных
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        public async Task DeleteCarAsync(int id) 
        {            
            try
            {
                var car = await _carRepository.GetCarByIdAsync(id);

                if (car == null)
                {
                    _logger.LogWarning($"Car with ID {id} not found.");
                    throw new EntityNotFoundException($"Car with ID {id} not found.");
                }

                _logger.LogInformation($"Car with ID {car.Id} successfully deleted.");

                await _carRepository.DeleteCarAsync(id);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while deleting the car with ID {id}. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Получает список автомобилей из базы данных
        /// </summary>
        /// <returns>Коллекция автомобилей</returns>
        public async Task<IEnumerable<Car>> GetAllCarsAsync() 
        {
            try
            {
                var cars = await _carRepository.GetAllCarsAsync();
                return cars.Any() ? cars : Enumerable.Empty<Car>();
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while retrieving cars. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Создает новый автомобиль в базе данных
        /// </summary>
        /// <param name="car">Автомобиль</param>
        /// <returns>Информация о новом автомобиле</returns>
        public async Task<Car> CreateCarAsync(Car car)
        {
            try
            {
                _logger.LogInformation("Creating car");

                var brand = await _brandService.GetBrandByNameAsync(car.Brand.Name);
                var carModel = await _carModelService.GetCarModelByNameAsync(car.CarModel.Name);
                var color = await _colorService.GetColorByNameAsync(car.Color.Name);

                //      NEED TO REFACTOR нет проверки на дубликат, добавлю позже

                var newCar = new Car
                {
                    BrandId = brand.Id,
                    CarModelId = carModel.Id,
                    ColorId = color.Id,
                    Amount = car.Amount,
                    IsAvailable = car.IsAvailable,
                };

                await _carRepository.CreateCarAsync(newCar);

                _logger.LogInformation($"Car with ID {newCar.Id} successfully created.");

                return newCar;        
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating car. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Обновляет доступность автомобиля
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="isAvaible">Доступность</param>
        public async Task<Car> UpdateCarAvailabilityAsync(int id, bool IsAvailable)
        {
            try
            {
                _logger.LogInformation($"Fetching car with ID {id}.");

                var existingCar = await _carRepository.GetCarByIdAsync(id);
                if (existingCar == null) 
                {
                    _logger.LogWarning($"Car with ID {id} not found.");
                    throw new EntityNotFoundException($"Car with ID {id} not found.");
                }

                existingCar.IsAvailable = IsAvailable;

                await _carRepository.UpdateCarAsync(existingCar);
                return existingCar;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while updating availability of car with ID {id}. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Обновляет количество автомобилей
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="amount">Количество</param>
        public async Task<Car> UpdateCarAmountAsync(int id, int amount)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(id);

            try 
            {
                if (existingCar == null)
                {
                    _logger.LogWarning($"Car with ID {id} not found.");
                    throw new EntityNotFoundException($"Car with ID {id} not found.");
                }

                if (amount < 0)
                {
                    _logger.LogWarning("Attempted to update a car with a negative amount.");
                    throw new ValidationErrorException("Amount cannot be negative.");
                }

                existingCar.Amount = amount;

                await _carRepository.UpdateCarAsync(existingCar);
                return existingCar;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating amount of car with ID {id}. Details: {ex.Message}");
                throw;
            }
        }
    }
}
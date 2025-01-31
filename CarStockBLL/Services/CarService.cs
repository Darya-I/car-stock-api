using CarStockBLL.CustomException;
using CarStockBLL.DTO.Car;
using CarStockBLL.Interfaces;
using CarStockBLL.Map;
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
        /// Экземпляр маппера
        /// </summary>
        private readonly CarMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над автомобилями
        /// </summary>
        /// <param name="carRepository">Репозиторий доступа к автомобилям</param>
        /// <param name="brandService">Сервис операций над маркой автомобиля</param>
        /// <param name="carModelService">Сервис операций над моделями автомобиля</param>
        /// <param name="colorService">Сервис операций над цветом автомобиля</param>
        /// <param name="logger">Логгер</param>
        /// <param name="mapper">Маппер</param>
        public CarService(
            ICarRepository<Car> carRepository, 
            IBrandService brandService, 
            ICarModelService carModelService, 
            IColorService colorService,
            ILogger<CarService> logger,
            CarMapper mapper)
        {
            _carRepository = carRepository;
            _brandService = brandService;
            _carModelService = carModelService;
            _colorService = colorService;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// Получает автомобиль из базы данных
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>DTO автомобиля</returns>
        public async Task<GetCarDTO> GetCarByIdAsync(int id)
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

                var result = _mapper.CarToGetCarDto(car);

                return result;

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
        /// <returns>DTO обновленного автомобиля</returns>
        public async Task<CarDTO> UpdateCarAsync(Car car)
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

                return (_mapper.CarToCarDto(car));
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
        /// <returns>Коллекция DTO автомобилей</returns>
        public async Task<List<GetCarDTO>> GetAllCarsAsync() 
        {
            try
            {
                var cars = await _carRepository.GetAllCarsAsync();
                var carsDto = cars.Any() ? cars : Enumerable.Empty<Car>();

                // К каждому элементу из списка применяется маппер
                return carsDto.Select(_mapper.CarToGetCarDto).ToList();
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
        /// <returns>DTO информация о новом автомобиле</returns>
        public async Task<CarDTO> CreateCarAsync(Car car)
        {
            try
            {
                _logger.LogInformation("Creating car");

                var brand = await _brandService.GetBrandByIdAsync(car.BrandId);
                var carModel = await _carModelService.GetCarModelByIdAsync(car.CarModelId);
                var color = await _colorService.GetColorByIdAsync(car.ColorId);

                var carExist = await _carRepository.CarExistAsync(brand.Id, carModel.Id, color.Id);

                if (carExist)
                {
                    _logger.LogWarning("This car already exist");
                    throw new EntityAlreadyExistsException("The car already exist");
                }

                var newCar = new Car()
                {
                    BrandId = brand.Id,
                    CarModelId = carModel.Id,
                    ColorId = color.Id,
                    Amount = car.Amount,
                    IsAvailable = car.IsAvailable,
                };

                await _carRepository.CreateCarAsync(newCar);

                _logger.LogInformation($"Car with ID {newCar.Id} successfully created.");

                // Из Model в DTO
                var result = _mapper.CarToCarDto(newCar);

                return result;
              
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
        /// <param name="isAvailable">Доступность</param>
        /// <returns>DTO доступности автомобиля</returns>
        public async Task<CarAvailabilityDTO> UpdateCarAvailabilityAsync(int id, bool isAvailable)
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

                existingCar.IsAvailable = isAvailable;

                await _carRepository.UpdateCarAsync(existingCar);

                var result = _mapper.CarAvailabilityUpdateDTO(existingCar);

                return result;
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
        /// <returns>DTO количества автомобиля</returns>
        public async Task<CarAmountDTO> UpdateCarAmountAsync(int id, int amount)
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

                return _mapper.CarAmountToDto(existingCar);
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
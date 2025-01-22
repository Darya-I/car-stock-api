using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarStockMAP;
using CarStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using CarStockMAP.DTO.Car;

namespace CarStockAPI.Controllers
{
    /// <summary>
    /// Контроллер для управления автомобилями
    /// Обрабатывает CRUD, количество и доступность автомобилей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        /// <summary>
        /// Экземпляр сервиса операций над автомобилями
        /// </summary>
        public readonly ICarService _carService;

        /// <summary>
        /// Экземляр сервиса маппинга автомобилей
        /// </summary>
        private readonly CarMapService _carMapService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<CarController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера автомобилей
        /// </summary>
        /// <param name="carService">Сервис операций над автомобилями</param>
        /// <param name="carMapService">Сервис маппинга автомобилей</param>
        /// <param name="logger">Логгер</param>
        public CarController(ICarService carService, 
                            CarMapService carMapService,
                            ILogger<CarController> logger)
        {
            _carService = carService;
            _carMapService = carMapService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка автомобилей
        /// </summary>
        /// <returns>Коллекция автомобилей</returns>
        [Authorize(Roles = "Admin, Manager, User")]
        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> GetAllCarsAsync()
        {
            _logger.LogInformation("Attempting to get cars");
            var carDtos = await _carMapService.GetMappedCarsAsync();
            _logger.LogInformation("Geting cars successful");
            return Ok(carDtos);
        }

        /// <summary>
        /// Создание нового автомобиля
        /// </summary>
        /// <param name="car">DTO автомобиля</param>
        /// <returns>Строку с информацией о добавленном автомобиле</returns>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost("CreateCar")]
        public async Task<ActionResult> CreateCarAsync(CarDTO car)
        {
            _logger.LogInformation("Attempting to create car");
            var result = await _carMapService.CreateMappedCarAsync(car);
            _logger.LogInformation("Creating car successful");
            return Ok(result);
        }

        /// <summary>
        /// Получение автомобиля по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>Автомобиль</returns>
        [Authorize(Roles = "Admin, Manager")]
        [HttpGet("GetCar/{id}")]
        public async Task<ActionResult> GetCarByIdAsync(int id)
        {
            _logger.LogInformation($"Attempting to get car with ID {id}");
            var result = await _carService.GetCarByIdAsync(id);
            _logger.LogInformation("Geting car successful");
            return Ok(result);
        }

        /// <summary>
        /// Обновление автомобиля
        /// </summary>
        /// <param name="carUpdateDto">DTO автомобиля для обновления</param>
        /// <returns>Результат обновления</returns>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("UpdateCar/{id}")]
        public async Task<IActionResult> UpdateCar([FromBody] CarUpdateDTO carUpdateDto)
        {
            _logger.LogInformation($"Attempting to update car with ID {carUpdateDto.Id}");

            if (carUpdateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var newUpdatedCar = await _carMapService.GetUpdatedMappedCarAsync(carUpdateDto);
            _logger.LogInformation("Updating car successful");
            return Ok(newUpdatedCar);
        }

        /// <summary>
        /// Удаление автомобиля
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>Результат удаления</returns>
        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            _logger.LogInformation($"Attempting to delete car with ID {id}");
            await _carService.DeleteCarAsync(id);
            _logger.LogInformation("Deleting car successful");
            return NoContent();
        }

        /// <summary>
        /// Обновление доступности автомобиля
        /// </summary>
        /// <param name="carAvailabilityUpdateDTO">DTO доступности</param>
        /// <returns>Результат изменения доступности, объект автомобиля</returns>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPatch("UpdateCarAvailability/{id}")]
        public async Task<IActionResult> UpdateCarAvailability([FromBody] CarAvailabilityUpdateDTO carAvailabilityUpdateDTO)
        {
            _logger.LogInformation($"Attempting to update availability for car with ID {carAvailabilityUpdateDTO.Id}");
            if (carAvailabilityUpdateDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            _logger.LogInformation("Updating availability of car successful");
            var result = await _carService.UpdateCarAvailabilityAsync(carAvailabilityUpdateDTO.Id, carAvailabilityUpdateDTO.IsAvailable);
            return Ok(result);
        }

        /// <summary>
        /// Обновление количества автомобиля
        /// </summary>
        /// <param name="carAmountUpdateDTO">DTO количества автомобиля</param>
        /// <returns>Результат изменения количества, объект автомобиля</returns>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPatch("UpdateCarAmount/{id}")]
        public async Task<IActionResult> UpdateCarAmount([FromBody] CarAmountUpdateDTO carAmountUpdateDTO)
        {
            _logger.LogInformation($"Attempting to update amount for car with ID {carAmountUpdateDTO.Id}");
            if (carAmountUpdateDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            _logger.LogInformation("Updating amount of car successful");
            var result = await _carService.UpdateCarAmountAsync(carAmountUpdateDTO.Id, carAmountUpdateDTO.Amount);
            return Ok(result);
        }
    }
}

﻿using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarStockBLL.Map;
using CarStockBLL.DTO.Car;

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
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<CarController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера автомобилей
        /// </summary>
        /// <param name="carService">Сервис операций над автомобилями</param>
        /// <param name="logger">Логгер</param>
        public CarController(ICarService carService,
                            ILogger<CarController> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка автомобилей
        /// </summary>
        /// <returns>Список автомобилей</returns>
        [Authorize(Policy = "ViewCarPolicy")]
        [HttpGet("GetCars")]
        public async Task<IActionResult> GetAllCarsAsync()
        {
            _logger.LogInformation("Attempting to get cars");

            var cars = await _carService.GetAllCarsAsync();

            _logger.LogInformation("Geting cars successful");
            
            return Ok(cars);
        }

        /// <summary>
        /// Создание нового автомобиля
        /// </summary>
        /// <param name="carDto">DTO автомобиля</param>
        /// <returns>DTO созданного автомобиля</returns>
        [Authorize(Policy = "CreateCarPolicy")]
        [HttpPost("CreateCar")]
        public async Task<IActionResult> CreateCarAsync(CarDTO carDto)
        {
            _logger.LogInformation("Attempting to create car");

            var mapper = new CarMapper();
            
            var car = mapper.CarDtoToCar(carDto);

            var newCar = await _carService.CreateCarAsync(car);

            _logger.LogInformation("Creating car successful");
            return Ok(newCar);
        }

        /// <summary>
        /// Получение автомобиля по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>DTO найденного автомобиля</returns>
        [Authorize(Policy = "ViewCarPolicy")]
        [HttpGet("GetCar/{id}")]
        public async Task<ActionResult> GetCarByIdAsync(int id)
        {
            _logger.LogInformation($"Attempting to get car with ID {id}");

            var carDto = await _carService.GetCarByIdAsync(id);

            _logger.LogInformation("Geting car successful");
           
            return Ok(carDto);
        }

        /// <summary>
        /// Обновление автомобиля
        /// </summary>
        /// <param name="carDTO">DTO автомобиля</param>
        /// <returns>Результат обновления</returns>
        [Authorize(Policy = "EditCarPolicy")]
        [HttpPut("UpdateCar/{id}")]
        public async Task<IActionResult> UpdateCar([FromBody] CarDTO carDTO)
        {
            _logger.LogInformation($"Attempting to update car with ID {carDTO.Id}");

            if (carDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            var mapper = new CarMapper();
            var car = mapper.CarDtoToCar(carDTO);
            var updatedCar = await _carService.UpdateCarAsync(car);

            _logger.LogInformation("Updating car successful");
            return Ok(updatedCar);
        }

        /// <summary>
        /// Удаление автомобиля
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>Результат удаления</returns>
        [Authorize(Policy = "DeleteCarPolicy")]
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
        /// <param name="carDto">DTO доступности автомобиля</param>
        /// <returns>Результат изменения доступности</returns>
        [Authorize(Policy = "EditCarPolicy")]
        [HttpPatch("UpdateCarAvailability/{id}")]
        public async Task<IActionResult> UpdateCarAvailability([FromBody] CarAvailabilityDTO carDto)
        {
            _logger.LogInformation($"Attempting to update availability for car with ID {carDto.Id}");
            if (carDto == null)
            {
                return BadRequest("Invalid data.");
            }

            _logger.LogInformation("Updating availability of car successful");
            
            var updatedCar = await _carService.UpdateCarAvailabilityAsync(carDto.Id, carDto.IsAvailable);
            return Ok(updatedCar);
        }

        /// <summary>
        /// Обновление количества автомобиля
        /// </summary>
        /// <param name="carDto">DTO количества автомобиля</param>
        /// <returns>Результат изменения количества</returns>
        [Authorize(Policy = "EditCarPolicy")]
        [HttpPatch("UpdateCarAmount/{id}")]
        public async Task<IActionResult> UpdateCarAmount([FromBody] CarAmountDTO carDto)
        {
            _logger.LogInformation($"Attempting to update amount for car with ID {carDto.Id}");
            if (carDto == null)
            {
                return BadRequest("Invalid data.");
            }

            _logger.LogInformation("Updating amount of car successful");
            var updatedCar = await _carService.UpdateCarAmountAsync(carDto.Id, carDto.Amount);
            return Ok(updatedCar);
        }
    }
}
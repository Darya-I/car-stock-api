using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarStockMAP;
using CarStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using CarStockMAP.DTO.Car;

namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly ICarService _carService;
        public readonly CarMapService _carMapService;
        private readonly ILogger<CarController> _logger;

        public CarController(ICarService carService, 
                            CarMapService carMapService,
                            ILogger<CarController> logger)
        {
            _carService = carService;
            _carMapService = carMapService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin, Manager, User")]
        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> GetAllCarsAsync()
        {
            _logger.LogInformation("Attempting to get cars");
            var carDtos = await _carMapService.GetMappedCarsAsync();
            _logger.LogInformation("Geting cars successful");
            return Ok(carDtos);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost("CreateCar")]
        public async Task<ActionResult> CreateCarAsync(CarDTO car)
        {
            _logger.LogInformation("Attempting to create car");
            var result = await _carMapService.CreateMappedCarAsync(car);
            _logger.LogInformation("Creating car successful");
            return Ok(result);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet("GetCar/{id}")]
        public async Task<ActionResult<CarViewModel>> GetCarByIdAsync(int id)
        {
            _logger.LogInformation("Attempting to get car with ID {id}", id);
            var result = await _carService.GetCarByIdAsync(id);
            _logger.LogInformation("Geting car successful");
            return Ok(result);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("UpdateCar/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarUpdateDTO carUpdateDto)
        {
            _logger.LogInformation("Attempting to update car with ID {id}", id);

            if (carUpdateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            if (id != carUpdateDto.Id)
            {
                return BadRequest("Car ID in the URL does not match the ID in the body.");
            }

            await _carMapService.GetUpdatedMappedCarAsync(carUpdateDto);
            _logger.LogInformation("Updating car successful");
            return Ok(carUpdateDto);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            _logger.LogInformation("Attempting to delete car with ID {id}",id);
            await _carService.DeleteCarAsync(id);
            _logger.LogInformation("Deleting car successful");
            return Ok($"Car with Id: {id} was deleted");
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPatch("UpdateCarAvailability/{id}")]
        public async Task<IActionResult> UpdateCarAvailability([FromRoute] int id, CarAvailabilityUpdateDTO carAvailabilityUpdateDTO)
        {
            _logger.LogInformation("Attempting to update availability for car with ID {id}", id);
            if (carAvailabilityUpdateDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            if (id != carAvailabilityUpdateDTO.Id)
            {
                return BadRequest("Car ID in the URL does not match the ID in the body.");
            }

            _logger.LogInformation("Updating availability of car successful");
            await _carService.UpdateCarAvailabilityAsync(carAvailabilityUpdateDTO.Id, carAvailabilityUpdateDTO.IsAvailable);
            return Ok($"The availability of car with ID {id} has been successfully updated. The current availability is: {carAvailabilityUpdateDTO.IsAvailable}.");
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPatch("UpdateCarAmount/{id}")]
        public async Task<IActionResult> UpdateCarAmount([FromRoute] int id, CarAmountUpdateDTO carAmountUpdateDTO)
        {
            _logger.LogInformation("Attempting to update amount for car with ID {id}", id);
            if (carAmountUpdateDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            if (id != carAmountUpdateDTO.Id)
            {
                return BadRequest("Car ID in the URL does not match the ID in the body.");
            }

            _logger.LogInformation("Updating amount of car successful");
            await _carService.UpdateCarAmountAsync(carAmountUpdateDTO.Id, carAmountUpdateDTO.Amount);
            return Ok($"The amount of car with ID {id} has been successfully updated. The current amount is: {carAmountUpdateDTO.Amount}.");
        }
    }
}

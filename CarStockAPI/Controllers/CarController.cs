using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarStockMAP;
using CarStockBLL.Infrastructure;
using CarStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarStockMAP.DTO.Car;



namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly ICarService _carService;
        public readonly CarMapService _carMapService;

        public CarController(ICarService carService, CarMapService carMapService)
        {
            _carService = carService;
            _carMapService = carMapService;
        }

        [Authorize(Roles = "Admin, Manager, User")]
        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> GetAllCarsAsync()
        {
            var carDtos = await _carMapService.GetMappedCarsAsync();
            return Ok(carDtos);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost("CreateCar")]
        public async Task<ActionResult> CreateCarAsync(CarDTO car)
        {
            var result = await _carMapService.CreateMappedCarAsync(car);

            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { message = result.Data });
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet("GetCar{id:int}")]
        public async Task<ActionResult<CarViewModel>> GetCarByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            return Ok(await _carService.GetCarByIdAsync(id));
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("UpdateCar/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarUpdateDTO carUpdateDto)
        {
            if (carUpdateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            if (id != carUpdateDto.Id)
            {
                return BadRequest("Car ID in the URL does not match the ID in the body.");
            }

            // 
            await _carMapService.GetUpdatedMappedCarAsync(carUpdateDto);
            return Ok(carUpdateDto);
        }

        //availability
        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("UpdateCarAvailability/{id}")]
        public async Task<IActionResult> UpdateCarAvailability(int id, [FromBody] CarUpdateDTO carUpdateDto)
        {
            if (carUpdateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            if (id != carUpdateDto.Id)
            {
                return BadRequest("Car ID in the URL does not match the ID in the body.");
            }
            await _carService.UpdateCarAvailabilityAsync(id, carUpdateDto.IsAvaible);
            return Ok(carUpdateDto);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _carService.DeleteCarAsync(id);
            return Ok($"Car with Id: {id} was deleted");
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("UpdateCarAmount/{id}")]
        public async Task<IActionResult> UpdateCarAmount(int id, [FromBody] CarUpdateDTO carUpdateDto)
        {
            if (carUpdateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            if (id != carUpdateDto.Id)
            {
                return BadRequest("Car ID in the URL does not match the ID in the body.");
            }

            await _carService.UpdateCarAmountAsync(id, carUpdateDto.Amount);
            return Ok(carUpdateDto);
        }
    }
}

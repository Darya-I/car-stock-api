using CarStockMAP.DTO;
using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarStockMAP;
using CarStockBLL.Infrastructure;
using CarStockBLL.Models;
using CarStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;



namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarController : ControllerBase
    {
        public readonly ICarService _carService;
        public readonly MapService _mapService;

        public CarController(ICarService carService, MapService mapService)
        {
            _carService = carService;
            _mapService = mapService;
        }

        [Authorize(Roles = "Admin, Manager, User")]
        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> GetAllCarsAsync()
        {
            var carDtos = await _mapService.GetMappedCarsAsync();
            return Ok(carDtos);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost("CreateCar")]
        public async Task<ActionResult> CreateCarAsync(CarDTO car)
        {
            var result = await _mapService.CreateMappedCarAsync(car);

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
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarUpdateDto carUpdateDto)
        {
            if (carUpdateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            if (id != carUpdateDto.Id)
            {
                return BadRequest("Car ID in the URL does not match the ID in the body.");
            }

            await _carService.UpdateCarAsync(carUpdateDto);
            return Ok(carUpdateDto);
        }

        //availability
        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("UpdateCarAvailability/{id}")]
        public async Task<IActionResult> UpdateCarAvailability(int id, [FromBody] CarUpdateDto carUpdateDto)
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
        public async Task<IActionResult> UpdateCarAmount(int id, [FromBody] CarUpdateDto carUpdateDto)
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

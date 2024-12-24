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
    public class CarController : ControllerBase
    {
        public readonly ICarService _carService;
        public readonly MapService _mapService;

        public CarController(ICarService carService, MapService mapService)
        {
            _carService = carService;
            _mapService = mapService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> GetAllCarsAsync()
        {
            var carDtos = await _mapService.GetMappedCarsAsync();
            return Ok(carDtos);
        }

        [HttpPost("CreateCar")]
        public async Task<ActionResult> CreateCarAsync(CarDTO car)
        {
            try
            {
                var result = await _mapService.CreateMappedCarAsync(car);

                if (!result.Success)
                {
                    return BadRequest(new { message = result.ErrorMessage });
                }

                return Ok(new { message = result.Data });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }

        [HttpGet("GetCar{id:int}")]
        public async Task<ActionResult<CarViewModel>> GetCarByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            return Ok(await _carService.GetCarByIdAsync(id));


        }

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

            try
            {
                
                await _carService.UpdateCarAsync(carUpdateDto);

                return Ok(carUpdateDto);
            }
            catch (ValidationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        //availability
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

            try
            {
                await _carService.UpdateCarAvailabilityAsync(id, carUpdateDto.IsAvaible);

                return Ok(carUpdateDto);
            }
            catch (ValidationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carService.DeleteCarAsync(id);
                return Ok($"Car with Id: {id} was deleted" );
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });  }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPut("UpdateCarAmoubt/{id}")]
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

            try
            {
                await _carService.UpdateCarAmountAsync(id, carUpdateDto.Amount);

                return Ok(carUpdateDto);
            }
            catch (ValidationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }


    }
}

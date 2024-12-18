﻿using CarStockMAP.DTO;
using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarStockMAP;
using Riok.Mapperly.Abstractions;
using CarStockBLL.Services;
using CarStockBLL.Models;
using CarStockBLL.Infrastructure;
//using CarStockDAL.Models;


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

        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> GetAllCarsAsync()
        {
            var carDtos = await _mapService.GetMappedCarsAsync();
            return Ok(carDtos);
        }

        [HttpPost("CreateCar")]
        public async Task<ActionResult> CreateCarAsync(CarViewModel car)
        {
            try
            {
                var result = await _carService.CreateCarAsync(car.BrandName, car.CarModelName, car.ColorName, car.Amount, car.IsAvaible);

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
            catch (Exception ex)
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
                return BadRequest(new { Message = ex.Message }); // Ошибка валидации (например, id не указан или объект не найден)
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred."); // В случае других ошибок
            }
        }







    }
}
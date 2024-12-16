using System.ComponentModel.DataAnnotations;
using CarStockAPI.DTO;
using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly ICarService _carService;
        
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetAllCarsAsync()
        {
            try
            {
                var cars = await _carService.GetAllCarsAsync();

                // Возвращаем результат с кодом 200 (OK)
                return Ok(cars);
            }
            catch (ValidationException ex)
            {
                // Если возникло исключение ValidationException
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Ловим любые другие исключения
                return StatusCode(500, new { Message = "Произошла ошибка на сервере.", Details = ex.Message });
            }
        }
    }
}

using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController : ControllerBase
    {
        /// <summary>
        /// Экземпляр сервиса операций над автомобилями
        /// </summary>
        private readonly ICarModelService _carModelService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<CarModelController> _logger;

        public CarModelController(ICarModelService carModelService, ILogger<CarModelController> logger)
        {
            _carModelService = carModelService;
            _logger = logger;
        }

        [HttpGet("GetModelByBrand/{id}")]
        public async Task<IActionResult> GetCarModelByBrandIdAsync([FromRoute] int id)
        {
            _logger.LogInformation("Attempting to get models");
            var carModel = await _carModelService.GetCarModelByBrandIdAsync(id);
            _logger.LogInformation("Geting models successful");
            return Ok(carModel);
        }

    }
}

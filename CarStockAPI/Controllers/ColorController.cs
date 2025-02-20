using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        /// <summary>
        /// Экземпляр сервиса операций над цветами
        /// </summary>
        private readonly IColorService _colorService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<ColorController> _logger;

        public ColorController(IColorService colorService, ILogger<ColorController> logger)
        {
            _colorService = colorService;
            _logger = logger;
        }

        /// <summary>
        /// Получает список цветов автомобиля
        /// </summary>
        /// <returns>Список цветов</returns>
        [HttpGet("GetColors")]
        public async Task<IActionResult> GetAllColorAsync()
        {
            _logger.LogInformation("Attempting to get colors");
            var colors = await _colorService.GetAllColorsAsync();
            _logger.LogInformation("Geting colors successful");
            return Ok(colors);
        }
    }
}

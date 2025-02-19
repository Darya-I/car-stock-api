using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        /// <summary>
        /// Экземпляр сервиса операций над автомобилями
        /// </summary>
        public readonly IBrandService _brandService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<BrandController> _logger;
        
        public BrandController(IBrandService brandService,
                               ILogger<BrandController> logger)
        {
            _brandService = brandService;
            _logger = logger;
        }

        [HttpGet("GetBrands")]
        public async Task<IActionResult> GetAllBrandsAsync()
        {
            _logger.LogInformation("Attempting to get brands");
            var brands = await _brandService.GetAllBrandsAsync();
            _logger.LogInformation("Geting brands successful");
            return Ok(brands);
        }
    }
}

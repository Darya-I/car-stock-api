using CarStockBLL.DTO.Admin;
using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    /// <summary>
    /// Контроллер для создания и удаления  информации о тех. работах
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// Экземпляр сервиса операций над тех. работами
        /// </summary>
        private readonly IMaitenanceService _maitenanceService;
        
        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<AdminController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера тех. работ
        /// </summary>
        /// <param name="maitenanceService">Сервис тех. работ</param>
        /// <param name="logger">Логгер</param>
        public AdminController(IMaitenanceService maitenanceService, ILogger<AdminController> logger) 
        {
            _maitenanceService = maitenanceService;
            _logger = logger;
        }

        /// <summary>
        /// Создание новой записи о тех. работах
        /// </summary>

        [HttpPost("CreateMaintenance")]
        public async Task<IActionResult> CreateMaintenance()
        {
            _logger.LogInformation("Attemting to create maintenance");
            await _maitenanceService.CreateMaintenanceAsync();
            _logger.LogInformation("Creating maintenance successful");
            return Ok();
        }

        /// <summary>
        /// Удаление записи о тех. работах по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор записи тех. работ</param>
        [HttpDelete("DeleteMaintenance/{id}")]
        public async Task<IActionResult> DeleteMaintenance([FromRoute] int id)
        {
            _logger.LogInformation("Attemting to create maintenance");
            await _maitenanceService.DeleteMaintenanceByIdAsync(id);
            _logger.LogInformation("Deleting maintenance successful");
            return Ok();
        }
    }
}
using CarStockMAP;
using CarStockMAP.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    /// <summary>
    /// Контроллер регистрации пользователей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// Экземляр сервиса маппинга пользователей
        /// </summary>
        public readonly UserMapService _userMapService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<RegisterController> _logger;

        /// <summary>
        /// Инициализирует экземляр контроллера регистрации
        /// </summary>
        /// <param name="userMapService">Сервис маппинга пользователей</param>
        /// <param name="logger">Логгер</param>
        public RegisterController(UserMapService userMapService,
                              ILogger<RegisterController> logger)
        {
            _userMapService = userMapService;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="createUserDTO">DTO для создания пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserDTO createUserDTO)
        {
            _logger.LogInformation("Attempting to register user");
            var result = await _userMapService.CreateMappedUserAsync(createUserDTO);
            _logger.LogInformation("Register user successful");
            return Ok(result);
        }
    }
}
using CarStockBLL.DTO.User;
using CarStockBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// Экземпляр сервиса операций над пользователями
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// Инициализирует экземляр контроллера регистрации
        /// </summary>
        /// <param name="userMapService">Сервис маппинга пользователей</param>
        /// <param name="logger">Логгер</param>
        public AccountController(IUserService userService,
                              ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="userDTO">DTO для обновления пользователя</param>
        /// <returns>Обновленный пользователь</returns>
        [Authorize(Policy = "CanEditAccount")]
        [HttpPatch("Edit")]
        public async Task<IActionResult> AccountEdit(UserDTO userDTO)
        {
            _logger.LogInformation("Attempting to register user");
            var result = await _userService.UpdateUserAccount(userDTO);
            _logger.LogInformation("Register user successful");
            return Ok(result);
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="userDTO">DTO для создания пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            _logger.LogInformation("Attempting to register user");
            var result = await _userService.RegisterUser(userDTO);
            _logger.LogInformation("Register user successful");
            return Ok(result);
        }

    }
}
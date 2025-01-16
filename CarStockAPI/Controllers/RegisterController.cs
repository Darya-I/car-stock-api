using CarStockBLL.Interfaces;
using CarStockMAP;
using CarStockMAP.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly UserMapService _userMapService;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(UserMapService userMapService,
                              IUserService userService,
                              ILogger<RegisterController> logger)
        {
            _userMapService = userMapService;
            _userService = userService;
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
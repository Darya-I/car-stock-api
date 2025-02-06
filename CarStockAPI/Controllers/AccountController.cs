using CarStockAPI.Filters;
using CarStockBLL.DTO.Account;
using CarStockBLL.Interfaces;
using CarStockBLL.Map;
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
        /// Экземпляр маппера
        /// </summary>
        private readonly UserMapper _mapper;

        /// <summary>
        /// Инициализирует экземляр контроллера регистрации
        /// </summary>
        /// <param name="userService">Сервис операций над пользователем</param>
        /// <param name="logger">Логгер</param>
        /// <param name="mapper">Маппер для модели пользователя</param>
        public AccountController(IUserService userService,
                              ILogger<AccountController> logger,
                              UserMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="request">Запрос обновления пользователя</param>
        /// <returns>Обновленный пользователь</returns>
        [ServiceFilter(typeof(RequireAcceptHeaderFilter))]
        [Authorize(Policy = "AccountPolicy")]
        [HttpPatch("Edit")]
        public async Task<IActionResult> AccountEdit(EditRequest request)
        {
            _logger.LogInformation("Attempting to register user");
            var user = _mapper.EditRequestToUser(request);
            var result = await _userService.UpdateUserAccount(user);
            _logger.LogInformation("Register user successful");
            return Ok(result);
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="request">Запрос для создания пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [ServiceFilter(typeof(RequireAcceptHeaderFilter))]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            _logger.LogInformation("Attempting to register user");
            var user = _mapper.RegisterRequestToUser(request);
            var result = await _userService.RegisterUser(user);
            _logger.LogInformation("Register user successful");
            return Ok(result);
        }

    }
}
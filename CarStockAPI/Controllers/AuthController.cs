using CarStockBLL.DTO.Auth;
using CarStockBLL.Interfaces;
using CarStockBLL.Map;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace CarStockAPI.Controllers
{
    /// <summary>
    /// Контроллер аутентификации и авторизации пользователей.
    /// Обрабатывает аутентификацию напрямую и через Гугл
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        /// <summary>
        /// Экземпляр сервиса авторизации пользователей
        /// </summary>
        private readonly IAuthorizeUserService _authorizationService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Экземпляр маппера
        /// </summary>
        private readonly UserMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера аутентификации
        /// </summary>
        /// <param name="authorizeUserService">Сервис авторизации пользователей</param>
        /// <param name="logger">Логгер</param>
        /// <param name="mapper">Маппер</param>
        public AuthController(IAuthorizeUserService authorizeUserService,
                              ILogger<AuthController> logger,
                              UserMapper mapper)
        {
            _authorizationService = authorizeUserService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Инициирует процесс входа через Google
        /// </summary>
        /// <returns>Перенаправление на страницу аутентификации Google</returns>
        [HttpGet("signin-google")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SignInWithGoogle() 
        {
            _logger.LogInformation("Initiating Google sign-in process.");
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Обрабатывает ответ от Google после аутентификации
        /// </summary>
        /// <returns>AccessToken или сообщение об ошибке</returns>
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            _logger.LogInformation("Receiving Google authentication response");
            // сначала логин в самом гугле, оттуда берем инфу
            var googleAuthResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!googleAuthResult.Succeeded)
            {
                _logger.LogWarning("Google authentication failed");
                return BadRequest();
            }
            //вот тут взяли
            var claims = googleAuthResult.Principal.Claims;

            //вот тут раскидали гугловского пользака
            GoogleLoginRequest googleUser = new GoogleLoginRequest
            {
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value.ToString(),
                Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value.ToString(),
            };

            var user = _mapper.GoogleRequestToUser(googleUser);

            _logger.LogInformation($"Processing login via google for user {googleUser.Email}");

            var accessToken = await _authorizationService.ProcessGoogle(user);

            _logger.LogInformation("Google login successful. Access token generated");
            return Ok(accessToken);
        }

        /// <summary>
        /// Вход пользователя
        /// </summary>
        /// <param name="request">Данные пользователя для входа</param>
        /// <returns>Access токен</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        { 
            _logger.LogInformation($"Attempting to login user {request.Email}");
            var user = _mapper.LoginRequestToUser(request);
            var result = await _authorizationService.Authenticate(user);
            _logger.LogInformation($"Authentication successful for user: {request.Email}. Setting refresh token cookie.");
            SetTokenCookie(result.RefreshToken);
            return Ok(result.AccessToken);
        }

        /// <summary>
        /// Обновляет refresh токен пользователя
        /// </summary>
        /// <param name="refreshTokenRequest">Refresh токен</param>
        /// <returns>Новый access токен</returns>
        [HttpPost("Refresh")] 
        public async Task<IActionResult> Refresh([FromBody] string refreshTokenRequest)
        {
            _logger.LogInformation("Attempting to refresh token for user with refresh token: {RefreshToken}", refreshTokenRequest);
            var user = await _authorizationService.GetUserByRefreshTokenAsync(refreshTokenRequest);
            if (user == null)
            {
                _logger.LogWarning($"No user found for refresh token: {refreshTokenRequest}");
                return Unauthorized();
            }
            _logger.LogInformation($"Access token generated successfully for user: {user.UserName}. Updating refresh token.");
            var result = await _authorizationService.UpdateRefreshTokenAsync(user);
            SetTokenCookie(result.Token);
            return Ok(result);
        }

        /// <summary>
        /// Устанавливает куки, содержащие refresh токен
        /// </summary>
        /// <param name="token">Refresh токен для хранения в куки</param>
        private void SetTokenCookie(string token)
        {
            var cookieOpt = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refresh-token", token, cookieOpt);
            _logger.LogInformation($"Refresh token cookie set successfully with expiration date: {cookieOpt.Expires}");
        }
    }
}
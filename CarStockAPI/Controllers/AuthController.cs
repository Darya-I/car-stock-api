using CarStockAPI.Models;
using CarStockBLL.Interfaces;
using CarStockMAP;
using CarStockMAP.DTO.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
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
        /// Экземпляр сервиса операций над пользователями
        /// </summary>
        private readonly IUserService _userService;
        
        /// <summary>
        /// Экземпляр сервиса генерации токенов
        /// </summary>
        private readonly ITokenService _tokenService;
        
        /// <summary>
        /// Экземляр сервиса маппинга пользователей
        /// </summary>
        private readonly UserMapService _useMapService;

        /// <summary>
        /// Экземпляр сервиса авторизации пользователей
        /// </summary>
        private readonly IAuthorizeUserService _authorizationService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера аутентификации
        /// </summary>
        /// <param name="userService">Сервис операций над пользователями</param>
        /// <param name="tokenService">Сервис работы с токенами</param>
        /// <param name="mapService">Сервис маппинга пользователей</param>
        /// <param name="authorizeUserService">Сервис авторизации пользователей</param>
        /// <param name="logger">Логгер</param>
        public AuthController(IUserService userService,
                              ITokenService tokenService,
                              UserMapService mapService,
                              IAuthorizeUserService authorizeUserService,
                              ILogger<AuthController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _useMapService = mapService;
            _authorizationService = authorizeUserService;
            _logger = logger;
        }

        /// <summary>
        /// Инициирует процесс входа через Google
        /// </summary>
        /// <returns>Перенаправление на страницу аутентификации Google</returns>
        [HttpGet("signin-google")]
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
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           
            if (!result.Succeeded)
            {
                _logger.LogWarning("Google authentication failed");
                return BadRequest();    
            }
            //вот тут взяли
            var claims = result.Principal.Claims;

            //вот тут раскидали гугловского пользака
            GoogleLoginRequestDTO googleUser = new GoogleLoginRequestDTO
            {
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value.ToString(),
                Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value.ToString(),
            };

            _logger.LogInformation($"Mapping Google user: {googleUser.Email}");
            var mapResult = await _useMapService.MapGoogle(googleUser);

            _logger.LogInformation("Google user mapping successful. Access token generated");
            return Ok(new
            {
                AccessToken = mapResult
            });
        }

        /// <summary>
        /// Вход пользователя
        /// </summary>
        /// <param name="loginRequest">Данные пользователя для входа</param>
        /// <returns>Токены пользователя</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            _logger.LogInformation($"Mapping user: {loginRequest.Email}");
            var response = await _useMapService.MapUserLogin(loginRequest);
            _logger.LogInformation($"Authentication successful for user: {loginRequest.Email}. Setting refresh token cookie.");
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        /// <summary>
        /// Обновляет refresh токен пользователя
        /// </summary>
        /// <param name="refreshTokenRequest">Refresh токен</param>
        /// <returns>Новый access токен</returns>
        [HttpPost("refresh")] 
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            _logger.LogInformation("Attempting to refresh token for user with refresh token: {RefreshToken}", refreshTokenRequest.RefreshToken);
            var user = await _authorizationService.GetUserByRefreshTokenAsync(refreshTokenRequest.RefreshToken);
            if (user == null)
            {
                _logger.LogWarning("No user found for refresh token: {RefreshToken}", refreshTokenRequest.RefreshToken);
                return Unauthorized();
            }

            _logger.LogInformation("User found. Retrieving roles for user: {UserName}", user.UserName);
            var roles = await _userService.GetUserRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            _logger.LogInformation("Generating access token for user: {UserName}", user.UserName);
            var token = _tokenService.GetAccessToken(claims, out DateTime expires);
            _logger.LogInformation("Access token generated successfully for user: {UserName}. Updating refresh token.", user.UserName);
            await _authorizationService.UpdateRefreshTokenAsync(user);
            return Ok(new { Token = token, Expiration = expires });
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

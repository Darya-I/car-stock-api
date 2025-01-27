using System.Security.Claims;
using CarStockBLL.CustomException;
using CarStockBLL.DTO.Auth;
using CarStockBLL.Interfaces;
using CarStockBLL.Map;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис авторизации и аутентификации пользователей
    /// </summary>
    public class AuthorizeUserService : IAuthorizeUserService
    {
        /// <summary>
        /// Экземпляр менеджера для работы с пользователями
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Экземпляр репозитория для работы с пользователями
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Экземпляр сервиса генерации токенов
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        public readonly ILogger<IAuthorizeUserService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса авторизации и аутентификации
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="tokenService">Сервис работы с токенами</param>
        /// <param name="logger">Логгер</param>
        public AuthorizeUserService(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ITokenService tokenService,
            ILogger<IAuthorizeUserService> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Аутентифицирует пользователя и генерирует токены
        /// </summary>
        /// <param name="requestDTO">DTO входа пользователя</param>
        /// <returns>Токены</returns>
        public async Task<LoginResponseDTO> Authenticate(LoginRequestDTO requestDTO)
        {
            try
            {
                var userFromDb = await _userRepository.GetUserByUsernameAsync(requestDTO.Email);
                if (userFromDb == null)
                {
                    _logger.LogWarning($"User with email {requestDTO.Email} not found.");
                    throw new InvalidUserDataException("Invalid email.");
                }
                if (!await _userManager.CheckPasswordAsync(userFromDb, requestDTO.Password))
                {
                    throw new InvalidUserDataException("Invalid password.");
                }

                // Получить роль
                var role = await _userRepository.GetUserRoleAsync(userFromDb.RoleId);

                // Получить клеймы для пользователя и его роли
                var claims = AssignClaim(userFromDb, role);

                var accessToken = _tokenService.GetAccessToken(claims, out var expires);
                var refreshToken = _tokenService.GetRefreshToken();

                // Присвоить refresh-токен пользователю
                userFromDb.RefreshToken = refreshToken;
                userFromDb.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

                await _userRepository.UpdateUserAsync(userFromDb);

                return new LoginResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error while authenticating user Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Получает пользователя из базы данных по refresh токену
        /// </summary>
        /// <param name="refreshToken">Значение refresh токена</param>
        /// <returns>Пользователь</returns>
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
                if (user == null) 
                {
                    _logger.LogWarning($"User with refresh token {refreshToken} not found");
                    throw new EntityNotFoundException($"User with refresh token {refreshToken} not found");
                }
                return user;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error while retrieving user with refresh token. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Обновляет refresh токен пользователя в базе данных
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Refresh токен и дату истечения</returns>
        public async Task<RefreshTokenResponseDTO> UpdateRefreshTokenAsync(User user)
        {
            try
            {
                // Получить роль
                var role = await _userRepository.GetUserRoleAsync(user.RoleId);

                // Получить клеймы для пользователя и его роли
                var claims = AssignClaim(user, role);

                var accessToken = _tokenService.GetAccessToken(claims, out var expires);

                var newRefreshToken = _tokenService.GetRefreshToken();
                var refreshTokenExpireTime = DateTime.UtcNow.AddDays(1);
                await _userRepository.UpdateRefreshTokenAsync(user, newRefreshToken, refreshTokenExpireTime);
                
                return new RefreshTokenResponseDTO 
                { 
                    Token = accessToken,
                    Expires = expires 
                };

            }

            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error while updating user`s refresh token Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Создает при необходимости и аутентифицирует пользователя от Гугла
        /// </summary>
        /// <param name="userDto">DTO пользователя от Гугла</param>
        /// <returns>Access токен</returns>
        public async Task<string> ProcessGoogle(GoogleLoginRequestDTO userDto)
        {
            try
            {
                var mapper = new UserMapper();
                var user = mapper.GoogleDtoToUser(userDto);

                var userFromDb = await _userRepository.GetUserByEmailAsync(userDto.Email);

                // Создать пользователя, если его нет
                if (userFromDb == null)
                {
                    // Добавить пользователя к роли "User"
                    user.RoleId = 2;
                    
                    var creationResult = await _userManager.CreateAsync(user);
                    if (!creationResult.Succeeded)
                    {
                        var info = string.Join("; ", creationResult.Errors.Select(e => e.Description));
                        _logger.LogError($"Failed to create a new user. Details: {info}");
                        throw new ApiException($"Failed to create a new user");
                    }

                    userFromDb = user;
                }

                if (userFromDb == null)
                {
                    _logger.LogWarning($"User with Email {user.Email} not found.");
                    throw new EntityNotFoundException("User not found and creation is not allowed.");
                }

                // Получить роль
                var role = await _userRepository.GetUserRoleAsync(userFromDb.RoleId);

                // Получить клеймы для пользователя и его роли
                var claims = AssignClaim(userFromDb, role);

                var accessToken = _tokenService.GetAccessToken(claims, out var expires);
                var refreshToken = _tokenService.GetRefreshToken();
                userFromDb.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

                // Присвоить refresh-токен пользователю
                userFromDb.RefreshToken = refreshToken;
                await _userRepository.UpdateUserAsync(userFromDb);

                return accessToken;
            }
            catch (ApiException) 
            { 
                throw; 
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Unexpected error while authenticating user with Google. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Вспомогательный метод для присваивания клеймов пользователю
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="role">Роль пользователя</param>
        /// <returns>Список клеймов</returns>
        private static List<Claim> AssignClaim(User user, Role role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (role != null)
            {
                var permissions = role.GetPermissions();

                claims.Add(new Claim(ClaimTypes.Role, role.Name));

                foreach (var permission in permissions)
                {
                    if (permission.Value)
                    {
                        claims.Add(new Claim("Permission", permission.Key));
                    }
                }
            }
            return claims;
        }
    }
}
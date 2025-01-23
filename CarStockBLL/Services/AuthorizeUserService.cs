using System;
using System.Security.Claims;
using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
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
        /// <param name="user">Пользователь</param>
        /// <returns>Пользователь и access токен</returns>
        public async Task<(User, string AccessToken)> Authenticate(User user)
        {
            try
            {
                var userFromDb = await _userRepository.GetUserByUsernameAsync(user.Email);
                if (userFromDb == null)
                {
                    _logger.LogWarning($"User with email {user.Email} not found.");
                    throw new InvalidUserDataException("Invalid email.");
                }
                if (!await _userManager.CheckPasswordAsync(userFromDb, user.PasswordHash))
                {
                    throw new InvalidUserDataException("Invalid password.");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.Email),
                };

                // Получить роль
                var role = await _userRepository.GetUserRolesAsync(userFromDb.RoleId);

                if (role != null)
                {
                    var permissions = GetRolePermissions(role);

                    claims.Add(new Claim(ClaimTypes.Role, role.Name));

                    foreach (var permission in permissions)
                    {
                        if (permission.Value)
                        {
                            claims.Add(new Claim("Permission", permission.Key));
                        }
                    }
                }

                var accessToken = _tokenService.GetAccessToken(claims, out var expires);

                var refreshToken = _tokenService.GetRefreshToken();

                // Присвоить refresh-токен пользователю
                userFromDb.RefreshToken = refreshToken;
                userFromDb.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

                await _userRepository.UpdateUserAsync(userFromDb);

                return (userFromDb, accessToken);
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
                return await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
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
        public async Task UpdateRefreshTokenAsync(User user)
        {
            try
            {
                var newRefreshToken = _tokenService.GetRefreshToken();
                var refreshTokenExpireTime = DateTime.UtcNow.AddDays(1);
                await _userRepository.UpdateRefreshTokenAsync(user, newRefreshToken, refreshTokenExpireTime);
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
        /// <param name="user">Пользователь</param>
        /// <returns>Access токен</returns>
        public async Task<string> ProcessGoogle(User user)
        {
            try
            {
                var userFromDb = await _userRepository.GetUserByEmailAsync(user.Email);
                if (userFromDb == null)
                {
                    // Создать пользователя, если его нет
                    var creationResult = await _userManager.CreateAsync(user);
                    if (!creationResult.Succeeded)
                    {
                        var info = string.Join("; ", creationResult.Errors.Select(e => e.Description));
                        _logger.LogError($"Failed to create a new user. Details: {info}");
                        throw new ApiException($"Failed to create a new user");
                    }

                    userFromDb = user;
                    // NEED TO REFACTOR на ишью маппинга
                    await _userManager.AddToRoleAsync(user, "User");
                    
                }

                if (userFromDb == null)
                {
                    _logger.LogWarning($"User with Email {user.Email} not found.");
                    throw new EntityNotFoundException("User not found and creation is not allowed.");
                }


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.Email),
                };

                // Получить роль
                var role = await _userRepository.GetUserRolesAsync(userFromDb.RoleId);

                if (role != null)
                {
                    var permissions = GetRolePermissions(role);

                    claims.Add(new Claim(ClaimTypes.Role, role.Name));

                    foreach (var permission in permissions)
                    {
                        if(permission.Value)
                        {
                            claims.Add(new Claim("Permission", permission.Key));
                        }
                    }
                }

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
        /// Вспомогательный метод для маппинга на строки возможности ролей
        /// </summary>
        /// <param name="role">Роль</param>
        /// <returns>Словарь с возможностями роли</returns>
        private static Dictionary<string, bool> GetRolePermissions(Role role)
        {
            var permissionsMap = new Dictionary<string, bool>
            {
                { "CanViewCar", role.CanViewCar },
                { "CanCreateCar", role.CanCreateCar },
                { "CanEditCar", role.CanEditCar },
                { "CanDeleteCar", role.CanDeleteCar },
                { "CanCreateUser", role.CanCreateUser },
                { "CanViewUser", role.CanViewUser },
                { "CanEditUser", role.CanEditUser },
                { "CanDeleteUser", role.CanDeleteUser }
            };

            return permissionsMap;
        }
    }
}
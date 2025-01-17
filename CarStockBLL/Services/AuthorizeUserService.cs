using System;
using System.Security.Claims;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CarStockBLL.Services
{
    public class AuthorizeUserService : IAuthorizeUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="tokenService">Сервис работы с токенами</param>
        public AuthorizeUserService(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
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
                    throw new UnauthorizedAccessException("Invalid email.");

                if (!await _userManager.CheckPasswordAsync(userFromDb, user.PasswordHash))
                    throw new UnauthorizedAccessException("Invalid password.");

                // Получить роль
                var roles = await _userManager.GetRolesAsync(userFromDb);

                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Email, userFromDb.Email),
                };

                if (roles.Any())
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.First()));
                }

                var accessToken = _tokenService.GetAccessToken(claims, out var expires);

                var refreshToken = _tokenService.GetRefreshToken();

                // Присвоить refresh-токен пользователю
                userFromDb.RefreshToken = refreshToken;
                userFromDb.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

                await _userRepository.UpdateUserAsync(userFromDb);

                return (userFromDb, accessToken);
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
                        throw new InvalidOperationException($"Failed to create a new user. Details: {info}");
                    }

                    userFromDb = user;
                    await _userManager.AddToRoleAsync(user, "User");
                }

                if (userFromDb == null)
                {
                    throw new KeyNotFoundException("User not found and creation is not allowed.");
                }

                // Получить роли
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Email, userFromDb.Email),
                };

                if (roles.Any())
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.First()));
                }

                var accessToken = _tokenService.GetAccessToken(claims, out var expires);
                var refreshToken = _tokenService.GetRefreshToken();
                userFromDb.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

                // Присвоить refresh-токен пользователю
                userFromDb.RefreshToken = refreshToken;
                await _userRepository.UpdateUserAsync(userFromDb);

                return accessToken;
            }
            catch (Exception) 
            {
                throw;
            }
        }
    }
}
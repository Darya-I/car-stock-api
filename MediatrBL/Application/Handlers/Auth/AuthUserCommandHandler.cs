using CarStockBLL.CustomException;
using CarStockBLL.DTO.Auth;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Common;
using MediatR;
using MediatrBL.Application.Commands.UserCommands;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Handlers.Auth
{
    /// <summary>
    /// Обработчик запроса на аутентификацию пользователя 
    /// </summary>
    public class AuthUserCommandHandler : IRequestHandler<AuthUserCommand, AuthResponse>
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
        public readonly ILogger<AuthUserCommandHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса авторизации и аутентификации
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="tokenService">Сервис работы с токенами</param>
        /// <param name="logger">Логгер</param>
        public AuthUserCommandHandler(UserManager<User> userManager,
                                    IUserRepository userRepository,
                                    ITokenService tokenService,
                                    ILogger<AuthUserCommandHandler> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на аутентификацию</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO входа</returns>
        /// <exception cref="InvalidUserDataException"></exception>
        public async Task<AuthResponse> Handle(AuthUserCommand request, CancellationToken cancellationToken)
        {
            var userFromDb = await _userRepository.GetUserByUsernameAsync(request.User.Email);
            if (userFromDb == null)
            {
                _logger.LogWarning($"User with email {request.User.Email} not found.");
                throw new InvalidUserDataException("Invalid email.");
            }
            if (!await _userManager.CheckPasswordAsync(userFromDb, request.User.PasswordHash))
            {
                throw new InvalidUserDataException("Invalid password.");
            }

            // Получить роль
            var role = await _userRepository.GetUserRoleAsync(userFromDb.RoleId);

            // Получить клеймы для пользователя и его роли
            var claims = ClaimHelper.AssignClaims(userFromDb, role);

            var accessToken = _tokenService.GetAccessToken(claims, out var expires);
            var refreshToken = _tokenService.GetRefreshToken();

            // Присвоить refresh-токен пользователю
            userFromDb.RefreshToken = refreshToken;
            userFromDb.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

            await _userRepository.UpdateUserAsync(userFromDb);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}

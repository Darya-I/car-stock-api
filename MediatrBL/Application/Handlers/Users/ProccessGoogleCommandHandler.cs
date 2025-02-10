using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Common;
using MediatR;
using MediatrBL.Application.Commands.UserCommands;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Handlers.Users
{
    /// <summary>
    /// Обработчик запроса на вход/регистрацию пользователя через Гугл
    /// </summary>
    public class ProccessGoogleCommandHandler : IRequestHandler<ProccessGoogleCommand, string>
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
        public readonly ILogger<ProccessGoogleCommandHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса авторизации и аутентификации
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="tokenService">Сервис работы с токенами</param>
        /// <param name="logger">Логгер</param>
        public ProccessGoogleCommandHandler(UserManager<User> userManager,
                                    IUserRepository userRepository,
                                    ITokenService tokenService,
                                    ILogger<ProccessGoogleCommandHandler> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на вход</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Access-токен</returns>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<string> Handle(ProccessGoogleCommand request, CancellationToken cancellationToken)
        {
            var userFromDb = await _userRepository.GetUserByEmailAsync(request.User.Email);

            // Создать пользователя, если его нет
            if (userFromDb == null)
            {
                // Добавить пользователя к роли "User"
                request.User.RoleId = 2;

                var creationResult = await _userManager.CreateAsync(request.User);
                if (!creationResult.Succeeded)
                {
                    var info = string.Join("; ", creationResult.Errors.Select(e => e.Description));
                    _logger.LogError($"Failed to create a new user. Details: {info}");
                    throw new ApiException($"Failed to create a new user");
                }

                userFromDb = request.User;
            }

            if (userFromDb == null)
            {
                _logger.LogWarning($"User with Email {request.User.Email} not found.");
                throw new EntityNotFoundException("User not found and creation is not allowed.");
            }

            // Получить роль
            var role = await _userRepository.GetUserRoleAsync(userFromDb.RoleId);

            // Получить клеймы для пользователя и его роли
            var claims = ClaimHelper.AssignClaims(userFromDb, role);

            var accessToken = _tokenService.GetAccessToken(claims, out var expires);
            var refreshToken = _tokenService.GetRefreshToken();
            userFromDb.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);

            // Присвоить refresh-токен пользователю
            userFromDb.RefreshToken = refreshToken;
            await _userRepository.UpdateUserAsync(userFromDb);

            return accessToken;
        }
    }
}
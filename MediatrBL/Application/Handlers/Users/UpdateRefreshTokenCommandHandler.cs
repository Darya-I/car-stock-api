using CarStockBLL.DTO.Auth;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using Common;
using MediatR;
using MediatrBL.Application.Commands.UserCommands;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Handlers.Users
{
    /// <summary>
    /// Обработчик запроса на обновление refresh-токена пользователя 
    /// </summary>
    public class UpdateRefreshTokenCommandHandler : IRequestHandler<UpdateRefreshTokenCommand, RefreshTokenResponse>
    {
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
        public readonly ILogger<UpdateRefreshTokenCommandHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса авторизации и аутентификации
        /// </summary>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="tokenService">Сервис работы с токенами</param>
        /// <param name="logger">Логгер</param>
        public UpdateRefreshTokenCommandHandler(IUserRepository userRepository,
                                    ITokenService tokenService,
                                    ILogger<UpdateRefreshTokenCommandHandler> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на обновление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO refresh-токена</returns>
        public async Task<RefreshTokenResponse> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Получить роль
            var role = await _userRepository.GetUserRoleAsync(request.User.RoleId);

            // Получить клеймы для пользователя и его роли
            var claims = ClaimHelper.AssignClaims(request.User, role);

            var accessToken = _tokenService.GetAccessToken(claims, out var expires);

            var newRefreshToken = _tokenService.GetRefreshToken();
            var refreshTokenExpireTime = DateTime.UtcNow.AddDays(1);
            await _userRepository.UpdateRefreshTokenAsync(request.User, newRefreshToken, refreshTokenExpireTime);

            return new RefreshTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                Expires = expires
            };
        }
    }
}
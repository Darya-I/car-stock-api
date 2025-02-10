using CarStockBLL.CustomException;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.UserQueries;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Handlers.Users
{
    /// <summary>
    /// Обработчик запроса на получение пользователя по refresh-токену
    /// </summary>
    public class GetUserByRefreshQueryHandler : IRequestHandler<GetUserByRefreshQuery, User>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с пользователями
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        public readonly ILogger<GetUserByRefreshQueryHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса авторизации и аутентификации
        /// </summary>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="logger">Логгер</param>
        public GetUserByRefreshQueryHandler(IUserRepository userRepository,
                                    ILogger<GetUserByRefreshQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Пользователь</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<User> Handle(GetUserByRefreshQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null)
            {
                _logger.LogWarning($"User with refresh token {request.RefreshToken} not found");
                throw new EntityNotFoundException($"User with refresh token {request.RefreshToken} not found");
            }
            return user;
        }
    }
}

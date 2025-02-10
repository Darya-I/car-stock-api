using CarStockBLL.DTO.Auth;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Commands.UserCommands;
using MediatrBL.Application.Queries.UserQueries;

namespace MediatrBL.Services
{
    /// <summary>
    /// Сервис аутентификации с использованием MediatR.
    /// Делегирует выполнение команд и запросов через MediatR
    /// </summary>
    public class AuthServiceWithMediatr : IAuthorizeUserService
    {
        /// <summary>
        /// Экземпляр MediatR для отправки команд и запросов.
        /// </summary>
        private readonly IMediator _mediator;

        public AuthServiceWithMediatr(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Аутентифицирует пользователя в системе
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        /// <returns>Ответ с токеном аутентификации</returns>
        public async Task<AuthResponse> Authenticate(User user)
        {
            return await _mediator.Send(new AuthUserCommand(user));
        }

        /// <summary>
        /// Получает пользователя по refresh-токену
        /// </summary>
        /// <param name="refreshToken">Refresh-токен</param>
        /// <returns>Объект пользователя</returns>
        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _mediator.Send(new GetUserByRefreshQuery(refreshToken));
        }

        /// <summary>
        /// Обрабатывает аутентификацию через Гугл
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        /// <returns>Токен доступа</returns>
        public async Task<string> ProcessGoogle(User user)
        {
            return await _mediator.Send(new ProccessGoogleCommand(user));
        }

        /// <summary>
        /// Обновляет refresh-токен пользователя
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        /// <returns>Обновленный refresh-токен</returns>
        public async Task<RefreshTokenResponse> UpdateRefreshTokenAsync(User user)
        {
            return await _mediator.Send(new UpdateRefreshTokenCommand(user));
        }
    }
}

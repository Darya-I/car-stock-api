using CarStockBLL.CustomException;
using CarStockBLL.Map;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Commands.UserCommands;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Handlers.Users
{
    /// <summary>
    /// Обработчик запроса на удаление пользователя 
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
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
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        /// <summary>
        /// Экземпляр маппера
        /// </summary>
        private readonly UserMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над пользователями
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="logger">Логгер</param>
        /// <param name="mapper">Маппер для модели пользователя</param>
        public DeleteUserCommandHandler(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ILogger<DeleteUserCommandHandler> logger,
            UserMapper mapper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на удаление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ApiException"></exception>
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                throw new EntityNotFoundException("A user with this email does not exists.");
            }

            var result = await _userManager.DeleteAsync(existingUser);
            if (!result.Succeeded)
            {
                _logger.LogError(string.Join($"Failed to delete the user. Errors:", result.Errors.Select(e => e.Description)));
                throw new ApiException("Failed to delete the user.");
            }
        }
    }
}

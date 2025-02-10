using CarStockBLL.CustomException;
using CarStockBLL.DTO.User;
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
    /// Обработчик запроса на обновление акккаунта пользователя 
    /// </summary>
    public class UpdateUserAccountCommandHandler : IRequestHandler<UpdateUserAccountCommand, GetUserDTO>
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
        private readonly ILogger<UpdateUserAccountCommandHandler> _logger;

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
        public UpdateUserAccountCommandHandler(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ILogger<UpdateUserAccountCommandHandler> logger,
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
        /// <param name="request">Запрос на обновление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO получения пользователя</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ApiException"></exception>
        public async Task<GetUserDTO> Handle(UpdateUserAccountCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.User.Email);

            if (existingUser == null)
            {
                throw new EntityNotFoundException("A user with this email does not exist.");
            }

            // Проверка и обновление имени пользователя
            if (!string.IsNullOrEmpty(request.User.UserName) && request.User.UserName != existingUser.UserName)
            {
                existingUser.UserName = request.User.UserName;
            }

            // Обновление пароля (если передан новый пароль)
            if (!string.IsNullOrEmpty(request.User.PasswordHash))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

                var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, request.User.PasswordHash);

                if (!passwordResult.Succeeded)
                {
                    _logger.LogError($"Failed to update user's password: {string.Join(", ", passwordResult.Errors.Select(e => e.Description))}");
                    throw new ApiException("Failed to update user's password.");
                }
            }

            // Обновление пользователя
            var result = await _userManager.UpdateAsync(existingUser);

            if (!result.Succeeded)
            {
                _logger.LogError($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                throw new ApiException("Failed to update user.");
            }

            return _mapper.UserToGetUserDto(existingUser);
        }
    }
}

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
    /// Обработчик запроса на обновление пользователя 
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserDTO>
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
        private readonly ILogger<UpdateUserCommandHandler> _logger;

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
        public UpdateUserCommandHandler(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ILogger<UpdateUserCommandHandler> logger,
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
        public async Task<GetUserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.UpdateUserDTO.Email);

            if (existingUser == null)
            {
                throw new EntityNotFoundException("A user with this email does not exist.");
            }

            var user = _mapper.UpadateDtoToUser(request.UpdateUserDTO);

            // Проверка и обновление роли
            if (user.RoleId > 0 && existingUser.RoleId != user.RoleId)
            {
                var role = await _userRepository.GetUserRoleAsync(user.RoleId);
                if (role == null)
                {
                    _logger.LogWarning($"Role with Id {user.RoleId} not found");
                    throw new EntityNotFoundException($"Role with Id {user.RoleId} not found");
                }
                existingUser.RoleId = user.RoleId;
            }

            // Проверка и обновление имени пользователя
            if (!string.IsNullOrEmpty(user.UserName) && user.UserName != existingUser.UserName)
            {
                existingUser.UserName = user.UserName;
            }

            // Проверка и обновление email
            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                existingUser.Email = user.Email;
            }

            // Обновление пароля (если передан новый пароль)
            if (!string.IsNullOrEmpty(request.UpdateUserDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

                var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, request.UpdateUserDTO.Password);

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
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
    /// Обработчик запроса на регистрацию пользователя 
    /// </summary>
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, GetUserDTO>
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
        private readonly ILogger<RegisterUserCommandHandler> _logger;

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
        public RegisterUserCommandHandler(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ILogger<RegisterUserCommandHandler> logger,
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
        /// <param name="request">Запрос на регистрацию</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO получения пользователя</returns>
        /// <exception cref="EntityAlreadyExistsException"></exception>
        /// <exception cref="ApiException"></exception>
        public async Task<GetUserDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.User.Email);

            if (existingUser != null)
            {
                throw new EntityAlreadyExistsException("A user with this email already exists.");
            }

            // Присваиваем роль "User" (объектом, чтобы потом сделать обратный маппинг)
            request.User.Role = await _userRepository.GetUserRoleAsync(2);
            var result = await _userManager.CreateAsync(request.User, request.User.PasswordHash);

            if (!result.Succeeded)
            {
                _logger.LogError(string.Join($"Failed to register the user. Errors:", result.Errors.Select(e => e.Description)));
                throw new ApiException("Failed to register the user. Errors:");
            }

            // С объекта нового пользователя на DTO
            var newUser = _mapper.UserToGetUserDto(request.User);

            return newUser;
        }
    }
}

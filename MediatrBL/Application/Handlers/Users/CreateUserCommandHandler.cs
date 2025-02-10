using MediatrBL.Application.Commands.UserCommands;
using CarStockBLL.DTO.User;
using MediatR;
using CarStockBLL.Map;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CarStockBLL.CustomException;

namespace MediatrBL.Application.Handlers.Users
{
    /// <summary>
    /// Обработчик запроса на создание пользователя 
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreareUserCommand, GetUserDTO>
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
        private readonly ILogger<CreateUserCommandHandler> _logger;

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
        /// 
        public CreateUserCommandHandler(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ILogger<CreateUserCommandHandler> logger,
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
        /// <param name="request">Запрос на создание</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO получения пользователя</returns>
        /// <exception cref="EntityAlreadyExistsException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ApiException"></exception>
        public async Task<GetUserDTO> Handle(CreareUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.CreateUserDTO.Email);
            
            if (existingUser != null)
            {
                throw new EntityAlreadyExistsException("A user with this email already exists.");
            }

            var role = await _userRepository.GetUserRoleAsync(request.CreateUserDTO.RoleId);

            if (role == null)
            {
                _logger.LogWarning($"Role with Id {request.CreateUserDTO.RoleId} not found");
                throw new EntityNotFoundException($"Role with Id {request.CreateUserDTO.RoleId} not found");
            }

            // С DTO на объект пользователя
            var user = _mapper.UserDtoToUser(request.CreateUserDTO);

            var result = await _userManager.CreateAsync(user, user.PasswordHash);

            if (!result.Succeeded)
            {
                _logger.LogError(string.Join($"Failed to create the user. Errors:", result.Errors.Select(e => e.Description)));
                throw new ApiException("Failed to create the user. Errors:");
            }

            // С объекта нового пользователя на DTO
            var newUser = _mapper.UserToGetUserDto(user);

            return newUser;
        }
    }
}
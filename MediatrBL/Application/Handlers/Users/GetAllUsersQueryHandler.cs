using CarStockBLL.CustomException;
using CarStockBLL.DTO.User;
using CarStockBLL.Map;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.UserQueries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Handlers.Users
{
    /// <summary>
    /// Обработчик запроса на получение списка пользователей
    /// </summary>
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<GetUserDTO>>
    {
        /// <summary>
        /// Экземпляр менеджера для работы с пользователями
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        /// <summary>
        /// Экземпляр маппера
        /// </summary>
        private readonly UserMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над пользователями
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="logger">Логгер</param>
        /// <param name="mapper">Маппер для модели пользователя</param>
        public GetAllUsersQueryHandler(
            UserManager<User> userManager,
            ILogger<GetAllUsersQueryHandler> logger,
            UserMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список DTO получения пользователей</returns>
        /// <exception cref="ApiException"></exception>
        public async Task<List<GetUserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                    .Include(u => u.Role) // Связные роли
                    .ToListAsync(cancellationToken);

            // К каждому элементу из списка применяется маппер
            var result = users.Select(_mapper.UserToGetUserDto).ToList();

            if (result == null)
            {
                _logger.LogError("Failed to retrieve users");
                throw new ApiException("Failed to retrieve users");
            }

            return result;
        }
    }
}

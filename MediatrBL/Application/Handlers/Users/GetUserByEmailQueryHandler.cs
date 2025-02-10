using CarStockBLL.CustomException;
using CarStockBLL.DTO.User;
using CarStockBLL.Map;
using CarStockDAL.Data.Interfaces;
using MediatR;
using MediatrBL.Application.Queries.UserQueries;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Handlers.Users
{
    /// <summary>
    /// Обработчик запроса на получение пользователя по почте
    /// </summary>
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserDTO>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с пользователями
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<GetUserByEmailQueryHandler> _logger;

        /// <summary>
        /// Экземпляр маппера
        /// </summary>
        private readonly UserMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над пользователями
        /// </summary>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="logger">Логгер</param>
        /// <param name="mapper">Маппер для модели пользователя</param>
        /// 
        public GetUserByEmailQueryHandler(
            IUserRepository userRepository,
            ILogger<GetUserByEmailQueryHandler> logger,
            UserMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Заполс на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO получения пользователя</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<GetUserDTO> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Email);

            if (user == null)
            {
                throw new EntityNotFoundException($"User with email '{request.Email}' was not found.");
            }

            return _mapper.UserToGetUserDto(user);
        }
    }
}
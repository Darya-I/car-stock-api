using CarStockBLL.DTO.User;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Commands.UserCommands;
using MediatrBL.Application.Queries.UserQueries;

namespace MediatrBL.Services
{
    /// <summary>
    /// Сервис операций с пользователями с использованием MediatR
    /// Делегирует выполнение команд и запросов через MediatR
    /// </summary>
    public class UserServiceWithMediatr : IUserService
    {
        /// <summary>
        /// Экземпляр MediatR для отправки команд и запросов.
        /// </summary>
        private readonly IMediator _mediator;

        public UserServiceWithMediatr(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Создает пользователя
        /// </summary>
        /// <param name="userDto">DTO создаваемого пользователя</param>
        /// <returns>DTO получения пользователя</returns>
        public async Task<GetUserDTO> CreateUserAsync(CreateUserDTO userDto)
        {
            return await _mediator.Send(new CreareUserCommand(userDto));
        }

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="email">Почта удаляемого пользователя</param>
        public async Task DeleteUserAsync(string email)
        {
            await _mediator.Send(new DeleteUserCommand(email));
        }

        /// <summary>
        /// Получает список пользователей
        /// </summary>
        /// <returns>Список DTO получения пользователей</returns>
        public async Task<List<GetUserDTO>> GetAllUsersAsync()
        {
            return await _mediator.Send(new GetAllUsersQuery());
        }

        /// <summary>
        /// Получает пользователя
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>DTO получения пользователя</returns>
        public async Task<GetUserDTO> GetUserAsync(string email)
        {
            return await _mediator.Send(new GetUserByEmailQuery(email));
        }

        /// <summary>
        /// Регистрирует пользователя
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        /// <returns>DTO получения пользователя</returns>
        public async Task<GetUserDTO> RegisterUser(User user)
        {
            return await _mediator.Send(new  RegisterUserCommand(user));
        }

        /// <summary>
        /// Обновляет аккаунт пользователя
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        /// <returns>DTO получения пользователя</returns>
        public async Task<GetUserDTO> UpdateUserAccount(User user)
        {
            return await _mediator.Send(new UpdateUserAccountCommand(user));
        }

        /// <summary>
        /// Обновляет пользоваетля
        /// </summary>
        /// <param name="userDto">DTO обновления пользователя</param>
        /// <returns>DTO получения пользователя</returns>
        public async Task<GetUserDTO> UpdateUserAsync(UpdateUserDTO userDto)
        {
            return await _mediator.Send(new UpdateUserCommand(userDto));
        }
    }
}

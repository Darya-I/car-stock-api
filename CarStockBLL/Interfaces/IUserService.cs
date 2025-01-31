using CarStockBLL.DTO.User;
using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса операций над пользователями
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получает пользователя с списком ролей из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        /// <returns>DTO представления созданного пользователя</returns>
        Task<GetUserDTO> GetUserAsync(string email);

        /// <summary>
        /// Создает нового пользователя в базе данных
        /// </summary>
        /// <param name="userDto">Пользователь</param>
        /// <returns>DTO представления созданного пользователя</returns>
        Task<GetUserDTO> CreateUserAsync(CreateUserDTO userDto);

        /// <summary>
        /// Удаляет пользователя из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        Task DeleteUserAsync(string email);

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="userDto">Пользователь</param>
        /// <returns>DTO представления обновленного пользователя</returns>
        Task<GetUserDTO> UpdateUserAsync(UpdateUserDTO userDto);

        /// <summary>
        /// Получает список пользователей из базы данных
        /// </summary>
        /// <returns>Список DTO представления пользователей</returns>
        Task<List<GetUserDTO>> GetAllUsersAsync();

        /// <summary>
        /// Создает нового пользователя и назначает ему роль User
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>DTO представления созданного пользователя</returns>
        Task<GetUserDTO> RegisterUser(User user);

        /// <summary>
        /// Обновляет только некоторые данные пользователя
        /// </summary>
        /// <param name="userDto">Пользователь</param>
        /// <returns>DTO представления обновленного пользователя</returns>
        Task<GetUserDTO> UpdateUserAccount(User user);
    }
}

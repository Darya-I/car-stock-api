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
        /// <returns>Пользователь и список его ролей</returns>
        Task<GetUserDTOdraft> GetUserAsync(string email);
        
        /// <summary>
        /// Получает список ролей пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Список ролей пользователя</returns>
        Task<List<string>> GetUserRolesAsync(User user);

        /// <summary>
        /// Создает нового пользователя в базе данных
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Созданный пользователь</returns>
        Task<GetUserDTOdraft> CreateUserAsync(UserDTO userDto);

        /// <summary>
        /// Удаляет пользователя из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        Task DeleteUserAsync(string email);
        
        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Обновленный пользователь</returns>
        Task<User> UpdateUserAsync(User user);
        
        /// <summary>
        /// Обновляет роль пользователя
        /// </summary>
        /// <param name="userEmail">Почта</param>
        /// <param name="newRole">Роль</param>
        Task UpdateUserRoleAsync(string userEmail, string newRole);
        
        /// <summary>
        /// Получает список пользователей и список их ролей из базы данных
        /// </summary>
        /// <returns>Список пользователей с их ролями</returns>
        Task<List<GetUserDTOdraft>> GetAllUsersAsync();
    }
}

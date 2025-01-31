using CarStockDAL.Models;

namespace CarStockDAL.Data.Interfaces
{
    /// <summary>
    /// Интерфейс для операций над данными пользователей
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получает пользователя по имени пользователя
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>Пользователь или <c>null</c>, если пользователь не найден</returns>
        Task<User?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Получает пользователя по refresh токену
        /// </summary>
        /// <param name="refreshToken">Refresh токен</param>
        /// <returns>Пользователь, связанный с указанным токеном обновления</returns>
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Обновляет refresh токен и срок его действия для указанного пользователя
        /// </summary>
        /// <param name="user">Пользователь, для которого обновляется токен</param>
        /// <param name="refrehToken">Новый refresh токен</param>
        /// <param name="refreshTokenExpireTime">Дата и время истечения срока действия токена</param>
        Task UpdateRefreshTokenAsync(User user, string refrehToken, DateTime refreshTokenExpireTime);

        /// <summary>
        /// Обновляет информацию о пользователе
        /// </summary>
        /// <param name="user">Обновленный объект пользователя</param>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Получает пользователя по электронной почте
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <returns>Пользователь или <c>null</c>, если пользователь не найден</returns>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Получает роль пользователя
        /// </summary>
        /// <param name="id">Идентификатор роли</param>
        /// <returns>Роль</returns>
        Task<Role> GetUserRoleAsync(int id);

    }
}
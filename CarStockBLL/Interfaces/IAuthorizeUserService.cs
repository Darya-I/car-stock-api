 using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса операций авторизации 
    /// </summary>
    public interface IAuthorizeUserService
    {
        /// <summary>
        /// Получает пользователя из базы данных по refresh токену
        /// </summary>
        /// <param name="refreshToken">Значение refresh токена</param>
        /// <returns>Пользователь</returns>
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        
        /// <summary>
        /// Обновляет refresh токен пользователя в базе данных
        /// </summary>
        /// <param name="user">Пользователь</param>
        Task UpdateRefreshTokenAsync(User user);
        
        /// <summary>
        /// Аутентифицирует пользователя и генерирует токены
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Пользователь и access токен</returns>
        Task<(User, string AccessToken)> Authenticate(User user);
        
        /// <summary>
        /// Создает при необходимости и аутентифицирует пользователя от Гугла
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Access токен</returns>
        Task<string> ProcessGoogle(User user);
    }
}

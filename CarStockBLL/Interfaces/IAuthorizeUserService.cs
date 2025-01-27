using CarStockBLL.DTO.Auth;
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
        /// <returns>Refresh токен и дату истечения</returns>
        Task<RefreshTokenResponseDTO> UpdateRefreshTokenAsync(User user);

        /// <summary>
        /// Аутентифицирует пользователя и генерирует токены
        /// </summary>
        /// <param name="requestDTO">DTO входа пользователя</param>
        /// <returns>Токены</returns>
        Task<LoginResponseDTO> Authenticate(LoginRequestDTO requestDTO);

        /// <summary>
        /// Создает при необходимости и аутентифицирует пользователя от Гугла
        /// </summary>
        /// <param name="userDto">DTO пользователя от Гугла</param>
        /// <returns>Access токен</returns>
        Task<string> ProcessGoogle(GoogleLoginRequestDTO userDto);
    }
}

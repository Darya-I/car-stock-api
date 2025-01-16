using System.Security.Claims;

namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса операций с токенами
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Генерирует access токен пользователя
        /// </summary>
        /// <param name="claims">Клеймы пользователя</param>
        /// <param name="expires">Время истечения</param>
        /// <returns>Access токен</returns>
        public string GetAccessToken(IEnumerable<Claim> claims, out DateTime expires);
        /// <summary>
        /// Генерирует refresh токен пользователя
        /// </summary>
        /// <returns>Refresh токен</returns>
        public string GetRefreshToken();
    }
}

namespace CarStockAPI.Models
{
    /// <summary>
    /// Модель запроса на обновление токена от клиента
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; }
    }
}

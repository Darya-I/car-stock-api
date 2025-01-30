namespace CarStockBLL.DTO.Auth
{
    /// <summary>
    /// Результата входа пользователя
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
namespace CarStockBLL.DTO.Auth
{
    /// <summary>
    /// Результат обновления refresh токена
    /// </summary>
    public class RefreshTokenResponse
    {
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        /// <summary>
        /// Дата истечения токена
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
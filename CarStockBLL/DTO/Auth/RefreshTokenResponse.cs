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
        public string Token { get; set; }

        /// <summary>
        /// Дата истечения токена
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
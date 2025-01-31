namespace CarStockBLL.DTO.Auth
{
    /// <summary>
    /// Модель запроса для входа пользователя
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}

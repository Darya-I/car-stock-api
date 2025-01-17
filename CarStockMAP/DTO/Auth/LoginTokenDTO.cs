namespace CarStockMAP.DTO.Auth
{
    /// <summary>
    /// DTO авторизованного пользователя с токенами
    /// </summary>
    public class LoginTokenDTO
    {
        /// <summary>
        /// Refresh токен
        /// </summary>
        public string RefreshToken { get; set; }
        
        /// <summary>
        /// Access токен
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }
    }
}

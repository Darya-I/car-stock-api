namespace CarStockMAP.DTO.Auth
{
    /// <summary>
    /// DTO авторизованного от Гугла пользователя
    /// </summary>
    public class GoogleLoginRequestDTO
    {
        /// <summary>
        /// Гугл-почта пользователя
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Имя пользователя от Гугла
        /// </summary>
        public string Name { get; set; }
    }
}

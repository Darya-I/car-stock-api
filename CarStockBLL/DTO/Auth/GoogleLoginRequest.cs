namespace CarStockBLL.DTO.Auth
{
    /// <summary>
    /// Модель запроса авторизованного от Гугла пользователя
    /// </summary>
    public class GoogleLoginRequest
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
namespace CarStockMAP.DTO.Auth
{
    /// <summary>
    /// DTO пользователя от Гугла
    /// </summary>
    public class GoogleUserDTO
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

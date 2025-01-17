namespace CarStockMAP.DTO.User
{
    /// <summary>
    /// DTO для создания нового пользователя
    /// </summary>
    public class CreateUserDTO
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
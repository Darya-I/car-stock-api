namespace CarStockBLL.DTO.User
{
    /// <summary>
    /// DTO для создания или обновления пользователя
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string RoleName { get; set; }
    }

}
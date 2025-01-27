namespace CarStockBLL.DTO.User
{
    /// <summary>
    /// DTO для создания пользователя
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

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public int RoleId { get; set; }
    }

}
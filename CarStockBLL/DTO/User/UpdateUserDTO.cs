namespace CarStockBLL.DTO.User
{
    /// <summary>
    /// DTO для обновления данных пользователя
    /// </summary>
    public class UpdateUserDTO
    {
        /// <summary>
        /// Новая почта пользователя
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Новый пароль пользователя
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Новое имя пользователя
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Идентификатор новой роли пользователя
        /// </summary>
        public int RoleId { get; set; }
    }
}

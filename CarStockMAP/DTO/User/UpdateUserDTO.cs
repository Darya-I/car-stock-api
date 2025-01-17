namespace CarStockMAP.DTO.User
{
    /// <summary>
    /// DTO для обновления данных пользователя.
    /// </summary>
    public class UpdateUserDTO
    {
        /// <summary>
        /// Новый почты пользователя
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
        /// Назначаемая или обновляемая роль пользователя
        /// </summary>
        public string? Role { get; set; }
    }
}
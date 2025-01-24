namespace CarStockBLL.DTO.User
{
    /// <summary>
    /// DTO для представления пользователя и его ролей
    /// </summary>
    public class GetUserDTOdraft
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Список ролей пользователя
        /// </summary>
        public string RoleName { get; set; }
    }
}
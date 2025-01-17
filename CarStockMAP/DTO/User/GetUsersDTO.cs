namespace CarStockMAP.DTO.User
{
    /// <summary>
    /// DTO для представления пользователя и его ролей
    /// </summary>
    public class GetUsersDTO
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
        public List<string> Roles { get; set; }
    }
}
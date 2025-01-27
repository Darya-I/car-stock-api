namespace CarStockBLL.DTO.User
{
    /// <summary>
    /// Общее DTO пользователя для регистрации и редактирования данных
    /// </summary>
    public class UserDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; } // При регистрации пароль валидируется
    }
}
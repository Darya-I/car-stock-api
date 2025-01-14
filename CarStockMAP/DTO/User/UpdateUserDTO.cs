namespace CarStockMAP.DTO.User
{
    /// <summary>
    /// DTO обновленного пользователя
    /// </summary>
    public class UpdateUserDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}

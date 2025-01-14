namespace CarStockMAP.DTO.User
{
    /// <summary>
    /// DTO нового пользователя
    /// </summary>
    public class CreateUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

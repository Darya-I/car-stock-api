namespace CarStockMAP.DTO.Auth
{
    /// <summary>
    /// DTO входа пользователя
    /// </summary>
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

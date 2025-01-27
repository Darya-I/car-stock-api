namespace CarStockBLL.DTO.Auth
{
    /// <summary>
    /// DTO для входа пользователя
    /// </summary>
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

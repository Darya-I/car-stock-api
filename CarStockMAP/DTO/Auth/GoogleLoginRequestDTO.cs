namespace CarStockMAP.DTO.Auth
{
    /// <summary>
    /// DTO авторизованного от Гугла пользователя
    /// </summary>
    public class GoogleLoginRequestDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Role { get; set; }
    }
}

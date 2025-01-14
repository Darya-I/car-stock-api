namespace CarStockMAP.DTO.Auth
{
    /// <summary>
    /// DTO пользователя от Гугла
    /// </summary>
    public class GoogleUserDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Role { get; set; }
    }
}

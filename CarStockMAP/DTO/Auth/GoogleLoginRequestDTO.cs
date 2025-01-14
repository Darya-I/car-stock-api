namespace CarStockMAP.DTO.Auth
{
    public class GoogleLoginRequestDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Role { get; set; }
    }
}

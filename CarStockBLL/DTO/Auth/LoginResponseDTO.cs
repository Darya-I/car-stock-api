namespace CarStockBLL.DTO.Auth
{
    /// <summary>
    /// DTO результата входа пользователя
    /// </summary>
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

namespace CarStockBLL.DTO.Auth
{
    /// <summary>
    /// DTO обновления refresh токена
    /// </summary>
    public class RefreshTokenResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}

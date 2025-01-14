namespace CarStockMAP.DTO.Auth
{
    /// <summary>
    /// DTO авторизованного пользователя с токенами
    /// </summary>
    public class LoginTokenDTO
    {
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}

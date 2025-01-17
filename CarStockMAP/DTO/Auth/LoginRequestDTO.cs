/// <summary>
/// DTO для запроса на вход пользователя
/// </summary>
public class LoginRequestDTO
{
    /// <summary>
    /// Почта пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; }
}
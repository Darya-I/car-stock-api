using System.ComponentModel.DataAnnotations;

namespace CarStockBLL.DTO.Account
{
    /// <summary>
    /// Модель запроса пользователя на регистрацию
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Юзернейм пользователя
        /// </summary>
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_-]{3,23}$",
            ErrorMessage = "Username must start with a letter and contain only English letters, numbers, '_' or '-'. Length: 4-24.")]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(16, ErrorMessage = "Password must be at most 16 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (@$!%*?&). Length: 8-16.")]
        public string Password { get; set; }
    }
}

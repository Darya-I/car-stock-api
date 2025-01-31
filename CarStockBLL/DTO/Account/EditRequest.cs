using System.ComponentModel.DataAnnotations;

namespace CarStockBLL.DTO.Account
{
    /// <summary>
    /// Модель запроса пользователя на редактирование данных
    /// </summary>
    public class EditRequest
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Юзернейм пользователя для смены
        /// </summary>
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Username can only contain English letters.")]
        public string? UserName { get; set; }

        /// <summary>
        /// Пароль пользователя для смены
        /// </summary>
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[\W_]).{6,}$",
        ErrorMessage = "Password must contain at least one digit, one uppercase letter, and one special character.")]
        public string? Password { get; set; }
    }
}
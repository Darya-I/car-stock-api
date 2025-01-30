using System.ComponentModel.DataAnnotations;

namespace CarStockBLL.DTO.User
{
    /// <summary>
    /// DTO для обновления данных пользователя
    /// </summary>
    public class UpdateUserDTO
    {
        /// <summary>
        /// Новая почта пользователя
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Новый пароль пользователя
        /// </summary>
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[\W_]).{6,}$",
        ErrorMessage = "Password must contain at least one digit, one uppercase letter, and one special character.")]
        public string? Password { get; set; }

        /// <summary>
        /// Новое имя пользователя
        /// </summary>
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Username can only contain English letters.")]
        public string? UserName { get; set; }

        /// <summary>
        /// Идентификатор новой роли пользователя
        /// </summary>
        public int RoleId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CarStockBLL.DTO.User
{
    /// <summary>
    /// DTO для создания пользователя
    /// </summary>
    public class CreateUserDTO
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[\W_]).{6,}$",
        ErrorMessage = "Password must contain at least one digit, one uppercase letter, and one special character.")]
        public string Password { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public int RoleId { get; set; }

    }

}
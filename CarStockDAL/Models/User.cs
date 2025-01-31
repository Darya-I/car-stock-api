using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CarStockDAL.Models
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string? RefreshToken { get; set; }
        
        /// <summary>
        /// Дата истечения токена обновления
        /// </summary>
        public DateTime RefreshTokenExpireTime { get; set; }
        
        /// <summary>
        /// Идентификатор роли пользователя
        /// </summary>
        public int RoleId { get; set; }
        
        /// <summary>
        /// Навигационное свойство для роли
        /// </summary>
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
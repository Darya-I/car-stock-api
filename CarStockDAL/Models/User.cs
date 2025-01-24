using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CarStockDAL.Models
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public int RoleId { get; set; }
        
        /// <summary>
        /// Навигационное свойство для роли
        /// </summary>
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CarStockDAL.Models
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }

        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
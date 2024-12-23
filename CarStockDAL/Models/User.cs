using Microsoft.AspNetCore.Identity;

namespace CarStockDAL.Models
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
    }
}

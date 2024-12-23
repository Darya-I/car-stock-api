using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data
{
    public class PostgreUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task UpdateRefreshTokenAsync(User user, string refrehToken, DateTime refreshTokenExpireTime)
        {
            user.RefreshToken = refrehToken;
            user.RefreshTokenExpireTime = refreshTokenExpireTime;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}

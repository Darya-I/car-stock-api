using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data
{
    public class PostgreUserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public PostgreUserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var test = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            return test;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task UpdateRefreshTokenAsync(User user, string refrehToken, DateTime refreshTokenExpireTime)
        {
            user.RefreshToken = refrehToken;
            user.RefreshTokenExpireTime = refreshTokenExpireTime;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }


    }
}

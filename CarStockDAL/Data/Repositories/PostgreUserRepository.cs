using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data.Repositories
{
    /// <summary>
    /// Репозиторий для операций с пользователями, реализующий интерфейс <see cref="IUserRepository">
    /// </summary>
    public class PostgreUserRepository : IUserRepository
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория пользователей
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateRefreshTokenAsync(User user, string refrehToken, DateTime refreshTokenExpireTime)
        {
            user.RefreshToken = refrehToken;
            user.RefreshTokenExpireTime = refreshTokenExpireTime;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext?.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

    }
}

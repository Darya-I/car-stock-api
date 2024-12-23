using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserAsync(string login, string password);

        public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

        public Task<List<string>> GetUserRolesAsync(User user);

        public Task UpdateRefreshTokenAsync(User user);

    }
}

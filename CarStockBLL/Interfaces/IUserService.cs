using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface IUserService
    {
        public Task<(User user, List<string> roles)?> GetUserAsync(string email);

        public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

        public Task<List<string>> GetUserRolesAsync(User user);

        public Task UpdateRefreshTokenAsync(User user);

        public Task<User> CreateUserAsync(User user);

        public Task DeleteUserAsync(string email);

        public Task<User> UpdateUserAsync(User user);

        public Task UpdateUserRoleAsync(string userEmail, string newRole);

        public Task<List<(User user, List<string> roles)>> GetAllUsersAsync();

        public Task<(User, string AccessToken)> Authenticate(User user);
    }
}

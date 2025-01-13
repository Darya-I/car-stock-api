using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface IUserService
    {
        Task<(User user, List<string> roles)?> GetUserAsync(string email);

        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

       Task<List<string>> GetUserRolesAsync(User user);

        Task UpdateRefreshTokenAsync(User user);

        Task<User> CreateUserAsync(User user);

        Task DeleteUserAsync(string email);

        Task<User> UpdateUserAsync(User user);

        Task UpdateUserRoleAsync(string userEmail, string newRole);

        Task<List<(User user, List<string> roles)>> GetAllUsersAsync();

        Task<(User, string AccessToken)> Authenticate(User user);

        Task<(User, string AccessToken)> GoogleAuthenticate(User user);

        Task<(User, string)> HandleGoogleUser(User user);
    }
}

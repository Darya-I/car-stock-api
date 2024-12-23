using CarStockBLL.Interfaces;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CarStockBLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManeger;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, ITokenService tokenService) 
        {
            _userManager = userManager;
            _roleManeger = roleManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<User?> GetUserAsync(string login, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(login);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            
            return null; // обработка в будущем
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task UpdateRefreshTokenAsync(User user)
        {
            var newRefreshToken = _tokenService.GetRefreshToken();
            var refreshTokenExpireTime = DateTime.Now.AddDays(1); // например, через 7 дней
            await _userRepository.UpdateRefreshTokenAsync(user, newRefreshToken, refreshTokenExpireTime);

        }
    }
}

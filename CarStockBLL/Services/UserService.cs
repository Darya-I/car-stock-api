using CarStockBLL.Interfaces;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CarStockBLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, ITokenService tokenService) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            var refreshTokenExpireTime = DateTime.UtcNow.AddDays(1); // например, через 7 дней
            await _userRepository.UpdateRefreshTokenAsync(user, newRefreshToken, refreshTokenExpireTime);

        }

        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to create the user. Errors: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(user, "User");

            return user;
        }

        public async Task DeleteUserAsync(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                throw new InvalidOperationException("A user with this email does not exists.");
            }
            var result = await _userManager.DeleteAsync(existingUser);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to delete the user. Errors: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser == null)
            {
                throw new InvalidOperationException("A user with this email does not exists.");
            }

            if (!string.IsNullOrEmpty(user.UserName) && user.UserName != existingUser.UserName) 
            {
                existingUser.UserName = user.UserName;
            }

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            { 
                user.Email = existingUser.Email; 
            }

            // Если указан новый пароль, обновляем его
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, user.PasswordHash);
                if (!passwordResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", passwordResult.Errors.Select(e => e.Description)));
                }
            }

            // Сохраняем изменения
            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return existingUser;
        }

        public async Task UpdateUserRoleAsync(string userEmail, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Удаляем текущие роли
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                throw new Exception($"Error removing roles: {string.Join("; ", removeResult.Errors.Select(e => e.Description))}");
            }

            // Проверяем, существует ли новая роль
            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                throw new InvalidOperationException($"Role {newRole} does not exist.");
            }

            // Добавляем новую роль
            var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addRoleResult.Succeeded)
            {
                throw new Exception($"Error adding role: {string.Join("; ", addRoleResult.Errors.Select(e => e.Description))}");
            }
        }

        //с вложенным списком для ролей
        public async Task<List<(User user, List<string> roles)>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<(User user, List<string> roles)>();

            foreach (var user in users)
            {
                // Получаем роли для каждого пользователя
                var roles = await _userManager.GetRolesAsync(user);
                result.Add((user, roles.ToList())); // Добавляем пользователя и его роли в результат
            }

            return result;
        }
    }
}

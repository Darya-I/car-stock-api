using System.ComponentModel.DataAnnotations;
using CarStockBLL.Interfaces;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис операций над пользователями
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Экземпляр менеджера для работы с пользователями
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Экземпляр менеджера для работы с ролями пользователей
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Экземпляр репозитория для работы с пользователями
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над пользователями
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="roleManager">Менеджер управления ролями пользователей</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        public UserService(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUserRepository userRepository) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Получает пользователя с списком ролей из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        /// <returns>Пользователь и список его ролей</returns>
        public async Task<(User user, List<string> roles)?> GetUserAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(email);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with email '{email}' was not found.");
                }

                // Получить роли для пользователя
                var roles = await _userManager.GetRolesAsync(user);        
                return (user, roles.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Получает список ролей пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Список ролей пользователя</returns>
        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            try 
            {
                return (await _userManager.GetRolesAsync(user)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Создает нового пользователя в базе данных
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Созданный пользователь</returns>
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    throw new ArgumentException("A user with this email already exists.");
                }
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (!result.Succeeded)
                {
                    // это неправильно
                    throw new ValidationException("Failed to create the user. Errors: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                await _userManager.AddToRoleAsync(user, "User");
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Удаляет пользователя из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        public async Task DeleteUserAsync(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                throw new ArgumentException("A user with this email does not exists.");
            }
            var result = await _userManager.DeleteAsync(existingUser);
            if (!result.Succeeded)
            {
                // не знаю что тут должно
                throw new InvalidOperationException("Failed to delete the user. Errors: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Обновленный пользователь</returns>
        public async Task<User> UpdateUserAsync(User user)
        {
            try
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

                // если указан новый пароль, обновляем его
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, user.PasswordHash);
                    if (!passwordResult.Succeeded)
                    {
                        throw new InvalidOperationException(string.Join("; ", passwordResult.Errors.Select(e => e.Description)));
                    }
                }

                var result = await _userManager.UpdateAsync(existingUser);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
                }

                return existingUser;
            }
            catch (Exception) 
            {
                throw;
            }
        }

        /// <summary>
        /// Обновляет роль пользователя
        /// </summary>
        /// <param name="userEmail">Почта</param>
        /// <param name="newRole">Роль</param>
        public async Task UpdateUserRoleAsync(string userEmail, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Удалить текущие роли
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                throw new InvalidOperationException($"Error removing roles: {string.Join("; ", removeResult.Errors.Select(e => e.Description))}");
            }

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                throw new KeyNotFoundException($"Role {newRole} does not exist.");
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addRoleResult.Succeeded)
            {
                throw new Exception($"Error adding role: {string.Join("; ", addRoleResult.Errors.Select(e => e.Description))}");
            }
        }

        /// <summary>
        /// Получает список пользователей и список их ролей из базы данных
        /// </summary>
        /// <returns>Список пользователей с их ролями</returns>
        public async Task<List<(User user, List<string> roles)>> GetAllUsersAsync()
        {
            //С вложенным списком для ролей
            try
            {
                var users = _userManager.Users.ToList();

                var result = new List<(User user, List<string> roles)>();

                foreach (var user in users)
                {
                    // Получить роли для каждого пользователя
                    var roles = await _userManager.GetRolesAsync(user);
                    result.Add((user, roles.ToList())); // Добавить пользователя и его роли в результат
                }
                return result;
            }
            catch (Exception) 
            {
                throw;
            }
        }
    }
}
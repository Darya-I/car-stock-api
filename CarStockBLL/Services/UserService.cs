﻿using System.ComponentModel.DataAnnotations;
using System.Data;
using CarStockBLL.CustomException;
using CarStockBLL.DTO.User;
using CarStockBLL.Interfaces;
using CarStockBLL.Map;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<IUserService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над пользователями
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="roleManager">Менеджер управления ролями пользователей</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="logger">Логгер</param>
        public UserService(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUserRepository userRepository,
            ILogger<IUserService> logger) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Получает пользователя с списком ролей из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        /// <returns>Пользователь и список его ролей</returns>
        public async Task<GetUserDTOdraft> GetUserAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(email);
                if (user == null)
                {
                    throw new EntityNotFoundException($"User with email '{email}' was not found.");
                }
                var mapper = new UserMapperBLL();

                return mapper.UserToGetUserDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving user. Details: {ex.Message}");
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
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving user`s roles. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Создает нового пользователя в базе данных
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Созданный пользователь</returns>
        public async Task<GetUserDTOdraft> CreateUserAsync(UserDTO userDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(userDto.Email);

                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("A user with this email already exists.");
                }

                var role = await _userRepository.GetRoleByNameAsync(userDto.RoleName);

                if (role== null)
                {
                    _logger.LogWarning($"Role with name {userDto.RoleName} not found");
                    throw new EntityNotFoundException($"Role with name {userDto.RoleName} not found");
                }

                var mapper = new UserMapperBLL();

                // С DTO на объект пользователя
                var user = mapper.UserDtoToUser(userDto);

                user.RoleId = role.Id;

                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                
                if (!result.Succeeded)
                {
                    // NEED TO FIX не отображает ошибку
                    _logger.LogError(string.Join($"Failed to create the user. Errors:", result.Errors.Select(e => e.Description)));
                    throw new ValidationErrorException("Failed to create the user. Errors:");
                }

                await _userManager.AddToRoleAsync(user, userDto.RoleName);

                // С объекта нового пользователя на DTO
                var newUser = mapper.UserToGetUserDto(user);

                return newUser;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating user. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Удаляет пользователя из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        public async Task DeleteUserAsync(string email)
        {
            try 
            {
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("A user with this email does not exists.");
                }

                var result = await _userManager.DeleteAsync(existingUser);
                if (!result.Succeeded)
                {
                    _logger.LogError(string.Join($"Failed to delete the user. Errors:", result.Errors.Select(e => e.Description)));
                    throw new ApiException("Failed to delete the user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting user. Details: {ex.Message}");
                throw;
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
                    throw new EntityNotFoundException("A user with this email does not exists.");
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
                        _logger.LogError(string.Join($"Failed to updating user`s password: ", passwordResult.Errors.Select(e => e.Description)));
                        throw new ApiException("Failed to updating user`s password");
                    }
                }

                var result = await _userManager.UpdateAsync(existingUser);
                if (!result.Succeeded)
                {
                    _logger.LogError(string.Join($"Failed to updating user: ", result.Errors.Select(e => e.Description)));
                    throw new ApiException("Failed to updating user");
                }

                return existingUser;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while updating user. Details: {ex.Message}");
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
            try
            {
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    _logger.LogWarning($"User with email {userEmail} does not exist.");
                    throw new EntityNotFoundException("User not found.");
                }

                var currentRoles = await _userManager.GetRolesAsync(user);

                // Удалить текущие роли
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    _logger.LogError(string.Join($"Failed to removing roles: ", removeResult.Errors.Select(e => e.Description)));
                    throw new ApiException($"Failed to removing roles");
                }

                if (!await _roleManager.RoleExistsAsync(newRole))
                {
                    _logger.LogWarning($"Role {newRole} does not exist.");
                    throw new EntityNotFoundException($"Role {newRole} does not exist.");
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
                if (!addRoleResult.Succeeded)
                {
                    _logger.LogError(string.Join($"Failed to adding role: ", addRoleResult.Errors.Select(e => e.Description)));
                    throw new ApiException($"Failed to adding role");
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while updating user`s roles. Details: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает список пользователей и список их ролей из базы данных
        /// </summary>
        /// <returns>Список пользователей с их ролями</returns>
        public async Task<List<GetUserDTOdraft>> GetAllUsersAsync()
        {
            
            try
            {
                var users = await _userManager.Users
                    .Include(u => u.Role) // Связные роли
                    .ToListAsync();

                var mapper = new UserMapperBLL();
                
                // К каждому элементу из списка применяется маппер
                var result = users.Select(mapper.UserToGetUserDto).ToList();

                if (result == null)
                {
                    _logger.LogError("Failed to retrieve users");
                    throw new ApiException("Failed to retrieve users");
                }

                return result;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while retrieving all users. Details: {ex.Message}");
                throw;
            }
        }
    }
}
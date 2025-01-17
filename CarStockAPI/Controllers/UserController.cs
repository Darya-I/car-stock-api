﻿using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using CarStockMAP;
using CarStockMAP.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    /// <summary>
    /// Контроллер управления пользователями CRUD
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Экземпляр сервиса операций над пользователями
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Экземляр сервиса маппинга пользователей
        /// </summary>
        public readonly UserMapService _userMapService;

        /// <summary>
        /// Экземляр логгера
        /// </summary>
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера пользователей
        /// </summary>
        /// <param name="userMapService">Сервис маппинга пользователей</param>
        /// <param name="userService">Сервис операций над пользователями</param>
        /// <param name="authorizeUserService">Сервис авторизации пользователей</param>
        /// <param name="logger">Логгер</param>
        public UserController(UserMapService userMapService, 
                              IUserService userService, 
                              IAuthorizeUserService authorizeUserService,
                              ILogger<UserController> logger)
        {
            _userMapService = userMapService;
            _userService = userService;
            _logger = logger;

        }
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="createUserDto">DTO для создания пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserDTO createUserDto)
        {
            _logger.LogInformation("Attempting to create user");
            var result = await _userMapService.CreateMappedUserAsync(createUserDto);
            _logger.LogInformation("Creating user successful");
            return Ok(result);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Результат удаления</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string email)
        {
            _logger.LogInformation("Attempting to delete user with email: {email}", email);
            await _userService.DeleteUserAsync(email);
            _logger.LogInformation("Deleting user successful");
            return Ok($"User with email: {email} was deleted");
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="updateUserDto">DTO пользователя для обновления</param>
        /// <returns>Результат обновления</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDto)
        {
            _logger.LogInformation("Attempting to update user with email: {email}",updateUserDto.Email);
            await _userMapService.UpdateMappedUserAsync(updateUserDto);
            _logger.LogInformation("Updating user successful");
            return Ok("User updated successfully.");
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns>Коллекция пользователй</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Attempting to get users");
            var users = await _userMapService.GetMappedUsersAsync();
            _logger.LogInformation("Geting users successful");
            return Ok(users);
        }

        /// <summary>
        /// Получение пользователя
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Пользователь</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUser/{email}")]
        public async Task<IActionResult> GetUser([FromRoute] string email)
        {
            _logger.LogInformation("Attempting to update user with email: {email}", email);
            var users = await _userMapService.GetMappedUserAsync(email);
            _logger.LogInformation("Geting user successful");
            return Ok(users);
        }
    }
}

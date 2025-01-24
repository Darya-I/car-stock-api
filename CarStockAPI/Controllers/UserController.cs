﻿using CarStockBLL.DTO.User;
using CarStockBLL.Interfaces;
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
        //[Authorize(Policy = "CreateUserPolicy")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDTO user)
        {
            _logger.LogInformation("Attempting to create user");
            //var result = await _userMapService.CreateMappedUserAsync(createUserDto);
            var result = await _userService.CreateUserAsync(user);
            _logger.LogInformation("Creating user successful");
            return Ok(result);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Результат удаления</returns>
        [Authorize(Policy = "DeleteUserPolicy")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string email)
        {
            _logger.LogInformation($"Attempting to delete user with email: {email}");
            await _userService.DeleteUserAsync(email);
            _logger.LogInformation("Deleting user successful");
            return Ok($"User with email: {email} was deleted");
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="updateUserDto">DTO пользователя для обновления</param>
        /// <returns>Результат обновления</returns>
        [Authorize(Policy = "EditUserPolicy")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDto)
        {
            _logger.LogInformation($"Attempting to update user with email: {updateUserDto.Email}");
            await _userMapService.UpdateMappedUserAsync(updateUserDto);
            _logger.LogInformation("Updating user successful");
            return Ok("User updated successfully.");
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns>Коллекция пользователй</returns>
        [Authorize(Policy = "ViewUserPolicy")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Attempting to get user");
            var result = await _userService.GetAllUsersAsync();
            _logger.LogInformation("Geting user successful");
            return Ok(result);
        }

        /// <summary>
        /// Получение пользователя
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Пользователь</returns>
        [Authorize(Policy = "ViewUserPolicy")]
        [HttpGet("GetUser/{email}")]
        public async Task<IActionResult> GetUser([FromRoute] string email)
        {
            _logger.LogInformation($"Attempting to update user with email: {email}");
            var user = await _userService.GetUserAsync(email);
            _logger.LogInformation("Retrive user successful");
            return Ok(user);
        }
    }
}
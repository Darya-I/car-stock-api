using CarStockBLL.Interfaces;
using CarStockMAP;
using CarStockMAP.DTO.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly UserMapService _userMapService;

        public UserController(UserMapService userMapService, IUserService userService)
        {
            _userMapService = userMapService;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserDTO createUserDto)
        {
            var result = await _userMapService.CreateMappedUserAsync(createUserDto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string email)
        {
            await _userService.DeleteUserAsync(email);
            return Ok($"User with email: {email} was murdered");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser/{email}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string email, [FromBody] UpdateUserDTO updateUserDto)
        {
            var user = await _userMapService.UpdateMappedUserAsync(updateUserDto);
            
            if (!string.IsNullOrWhiteSpace(updateUserDto.Role))
            {
                await _userService.UpdateUserRoleAsync(user.Email, updateUserDto.Role);
            }
            return Ok("User updated successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userMapService.GetMappedUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUser/{email}")]
        public async Task<IActionResult> GetUser([FromRoute] string email)
        {
            var users = await _userMapService.GetMappedUserAsync(email);
            return Ok(users);
        }
    }
}

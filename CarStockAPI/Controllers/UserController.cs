using CarStockBLL.Interfaces;
using CarStockMAP;
using CarStockMAP.DTO;
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
        public readonly MapService _mapService;

        public UserController(MapService mapService, IUserService userService)
        {
            _mapService = mapService;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserDTO createUserDto)
        {
            var result = await _mapService.CreateMappedUserAsync(createUserDto);
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
            var user = await _mapService.UpdateMappedUserAsync(updateUserDto);
            
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
            var users = await _mapService.GetMappedUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetUser/{email}")]
        public async Task<IActionResult> GetUser([FromRoute] string email)
        {
            var users = await _mapService.GetMappedUserAsync(email);
            return Ok(users);
        }
    }
}

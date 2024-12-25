using CarStockAPI.Models;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace CarStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AuthController(IUserService userService, ITokenService tokenService, UserManager<User> userManager)
        {
            _userService = userService;
            _tokenService = tokenService;
            _userManager = userManager;
        }


        //                                                  пользователю надо в бд сохранять его токен и времяжизни
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };
            var roles = await _userService.GetUserRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //мб тут надо await
            var token = _tokenService.GetAccessToken(claims, out DateTime expires);

            return Ok(new { Token = token, Expiration = expires });

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(refreshTokenRequest.RefreshToken);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await _userService.GetUserRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = _tokenService.GetAccessToken(claims, out DateTime expires);
            await _userService.UpdateRefreshTokenAsync(user);
            return Ok(new { Token = token, Expiration = expires });
        }
    }
}

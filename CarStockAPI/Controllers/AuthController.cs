using CarStockAPI.Models;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using CarStockMAP;
using CarStockMAP.Models;
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
        public readonly MapService _mapService;


        public AuthController(IUserService userService, ITokenService tokenService, UserManager<User> userManager, MapService mapService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _userManager = userManager;
            _mapService = mapService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _mapService.MapUserLogin(loginRequest);
            setTokenCookie(response.RefreshToken);
            return Ok(response);
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

        // хелп метод
        private void setTokenCookie(string token)
        {
            var cookieOpt = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refresh-token", token, cookieOpt);
        }
    }
}

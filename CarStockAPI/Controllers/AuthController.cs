﻿using CarStockAPI.Models;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using CarStockMAP;
using CarStockMAP.DTO.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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
        public readonly UserMapService _mapService;
        //public readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="tokenService"></param>
        /// <param name="mapService"></param>
        public AuthController(IUserService userService, ITokenService tokenService, UserMapService mapService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapService = mapService;
            //_logger = logger;
        }


        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle() 
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return BadRequest();    
            }

            var claims = result.Principal.Claims;

            GoogleLoginRequestDTO googleUser = new GoogleLoginRequestDTO
            {
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value.ToString(),
                Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value.ToString(),
            };

            await _mapService.MapGoogleUser(googleUser);

            var response = await _mapService.MapGoogleUserLogin(googleUser);

            //_logger.LogInformation("balagdakfliadshpf");

            return Ok(new
            {
                Claims = claims.Select(c => new { Type = c.Type, Value = c.Value }),
                AccessToken = response.Token
            });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
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

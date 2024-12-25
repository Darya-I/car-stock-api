using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CarStockBLL.Services
{
    public class TokenService : ITokenService
    {
        public readonly string _secretKey;
        public readonly string _issuer;
        public readonly string _audience;
        private readonly IConfiguration _configuration;


        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("JwtConfig:Secret");
            _issuer = configuration.GetValue<string>("JwtConfig:Issuer");
            _audience = configuration.GetValue<string>("JwtConfig:Audience");            
        }

        
        public string GetAccessToken(IEnumerable<Claim> claims, out DateTime expires)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            expires = DateTime.UtcNow.AddMinutes(15);

            JwtSecurityToken token = new(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            //освобождаем тут unmanagment от объекта RandomNumberGenerator
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber);
        }
    }
}

//iss: чувствительная к регистру строка или URI, которая является
//уникальным идентификатором стороны, генерирующей токен (issuer).

//aud: массив чувствительных к регистру строк или URI, являющийся
//списком получателей данного токена. Когда принимающая сторона получает JWT
//с данным ключом, она должна проверить наличие себя в получателях — иначе проигнорировать токен (audience).
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CarStockBLL.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CarStockBLL.Services
{
    public class TokenService : ITokenService
    {
        public readonly string _secretKey;
        public readonly string _issuer;
        public readonly string _audience;
        public readonly ILogger<ITokenService> _logger;

        public TokenService(
            string secretKey,
            string issuer,
            string audience,
            ILogger<ITokenService> logger)
        {    
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _logger = logger;
        }
        
        public string GetAccessToken(IEnumerable<Claim> claims, out DateTime expires)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_secretKey))
                {
                    _logger.LogError("Secret key is missing or invalid");
                    throw new SecurityTokenEncryptionFailedException("Secret key is missing or invalid.");
                }

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
                _logger.LogInformation("Access token succesfully genereted");
                return handler.WriteToken(token);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Invalid argument provided for JWT creation. Details: {Details}", ex.Message);
                throw new InvalidOperationException("Failed to create access token due to invalid parameters.");
            }
            catch (Exception ex) 
            {
                _logger.LogError("Unexpected error while generating access token. Details: {Details}", ex.Message);
                throw;
            }
        }

        public string GetRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];
                //освобождаем тут unmanagment от объекта RandomNumberGenerator
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                _logger.LogInformation("Refresh token succesfully genereted");
                return Convert.ToHexString(randomNumber);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError("Unexpected error while generating refresh token. Details: {Details}", ex.Message);
                throw;
            }
        }
    }
}

//iss: чувствительная к регистру строка или URI, которая является
//уникальным идентификатором стороны, генерирующей токен (issuer).

//aud: массив чувствительных к регистру строк или URI, являющийся
//списком получателей данного токена. Когда принимающая сторона получает JWT
//с данным ключом, она должна проверить наличие себя в получателях — иначе проигнорировать токен (audience).
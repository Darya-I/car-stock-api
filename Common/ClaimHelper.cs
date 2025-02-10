using System.Security.Claims;
using CarStockDAL.Models;

namespace Common
{
    /// <summary>
    /// Класс-помощник для использования в сервисах авторизации
    /// </summary>
    public static class ClaimHelper
    {
        /// <summary>
        /// Вспомогательный метод для присваивания клеймов пользователю
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="role">Роль пользователя</param>
        /// <returns>Список клеймов</returns>
        public static List<Claim> AssignClaims(User user, Role role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (role != null)
            {
                var permissions = role.GetPermissions();

                claims.Add(new Claim(ClaimTypes.Role, role.Name));

                foreach (var permission in permissions)
                {
                    if (permission.Value)
                    {
                        claims.Add(new Claim("Permission", permission.Key));
                    }
                }
            }
            return claims;
        }
    }
}
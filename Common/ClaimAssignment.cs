using System.Security.Claims;
using CarStockDAL.Models;

// Общие файлы проекта
namespace Common
{
    public static class ClaimAssignment
    {
        /// <summary>
        /// Вспомогательный метод для присваивания клеймов пользователю
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="role">Роль пользователя</param>
        /// <returns>Список клеймов</returns>
        public static List<Claim> AssignClaimAsync(User user, Role role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (role != null)
            {
                var permissions = RolePermissionMapper.GetRolePermissions(role);

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

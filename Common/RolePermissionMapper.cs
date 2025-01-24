using CarStockDAL.Models;

// Общие файлы проекта
namespace Common
{
    public static class RolePermissionMapper
    {
        /// <summary>
        /// Вспомогательный метод для маппинга на строки возможности ролей
        /// </summary>
        /// <param name="role">Роль</param>
        /// <returns>Словарь с возможностями роли</returns>
        public static Dictionary<string, bool> GetRolePermissions(Role role)
        {
            var permissionsMap = new Dictionary<string, bool>
            {
                { "CanViewCar", role.CanViewCar },
                { "CanCreateCar", role.CanCreateCar },
                { "CanEditCar", role.CanEditCar },
                { "CanDeleteCar", role.CanDeleteCar },
                { "CanCreateUser", role.CanCreateUser },
                { "CanViewUser", role.CanViewUser },
                { "CanEditUser", role.CanEditUser },
                { "CanDeleteUser", role.CanDeleteUser }
            };

            return permissionsMap;
        }
    }
}
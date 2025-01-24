using System.Text.Json.Serialization;

namespace CarStockDAL.Models
{
    /// <summary>
    /// Кастомные роли
    /// </summary>
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        /// <summary>
        /// Допустимость просмотра автомобиля
        /// </summary>
        public bool CanViewCar { get; set; } = true;

        /// <summary>
        /// Допустимость создания автомобиля
        /// </summary>
        public bool CanCreateCar { get; set; } = false;

        /// <summary>
        /// Допустимость редактирования автомобиля
        /// </summary>
        public bool CanEditCar { get; set; } = false;

        /// <summary>
        /// Допустимость удаления автомобиля
        /// </summary>
        public bool CanDeleteCar { get; set; } = false;

        /// <summary>
        /// Допустимость создания пользователя 
        /// </summary>
        public bool CanCreateUser { get; set; } = false;

        /// <summary>
        /// Допустимость просмотра пользователя
        /// </summary>
        public bool CanViewUser { get; set; } = false;

        /// <summary>
        /// Допустимость редактирования пользователя
        /// </summary>
        public bool CanEditUser { get; set; } = false;

        /// <summary>
        /// Допустимость удаления пользователя
        /// </summary>
        public bool CanDeleteUser { get; set; } = false;

        /// <summary>
        /// Связные пользователи
        /// </summary>
        [JsonIgnore]
        public ICollection<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// Получение прав роли в виде словаря
        /// </summary>
        /// <returns>Словарь с возможностями роли</returns>
        public Dictionary<string, bool> GetPermissions()
        {
            return new Dictionary<string, bool>
            {
                { "CanViewCar", CanViewCar },
                { "CanCreateCar", CanCreateCar },
                { "CanEditCar", CanEditCar },
                { "CanDeleteCar", CanDeleteCar },
                { "CanCreateUser", CanCreateUser },
                { "CanViewUser", CanViewUser },
                { "CanEditUser", CanEditUser },
                { "CanDeleteUser", CanDeleteUser }
            };
        }
    }
}
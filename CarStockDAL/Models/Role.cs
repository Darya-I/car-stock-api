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
        
        public bool CanViewCar { get; set; } = true;        
        public bool CanCreateCar { get; set; } = false;
        public bool CanEditCar { get; set; } = false;
        public bool CanDeleteCar { get; set; } = false;
 
        public bool CanCreateUser { get; set; } = false;
        public bool CanViewUser { get; set; } = false;        
        public bool CanEditUser { get; set; } = false;       
        public bool CanDeleteUser { get; set; } = false;

        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
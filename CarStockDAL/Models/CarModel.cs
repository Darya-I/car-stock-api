using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace CarStockDAL.Models
{
    public class CarModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public int BrandId { get; set; } //required FK
        public Brand Brand { get; set; }
        [JsonIgnore]
        public ICollection<Color> Colors { get; set; } = new List<Color>();

    }
}

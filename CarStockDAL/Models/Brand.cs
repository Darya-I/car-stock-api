using System.Text.Json.Serialization;

namespace CarStockDAL.Models
{
    public class Brand
    {
        /// <summary>
        /// Идентификатор марки автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название марки автомобиля
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Коллекция автомобилей определенной марки
        /// </summary>
        [JsonIgnore]
        public ICollection<Car> Cars { get; set; } = new List<Car>();
        
        /// <summary>
        /// Коллекция моделей определенной марки
        /// </summary>
        [JsonIgnore]
        public ICollection<CarModel> CarsModel { get; set; } = new List<CarModel>();

    }
}
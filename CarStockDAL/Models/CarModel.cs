using System.Text.Json.Serialization;

namespace CarStockDAL.Models
{
    public class CarModel
    {
        /// <summary>
        /// Идентификатор модели автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название модели автомобиля
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентифкатор связанной марки автомобиля
        /// </summary>
        public int BrandId { get; set; } //required FK

        /// <summary>
        /// Навигационное свойство к марке автомобиля
        /// </summary>
        public Brand Brand { get; set; }

        /// <summary>
        /// Коллекция цветов у модели автомобиля
        /// </summary>
        [JsonIgnore]
        public ICollection<Color> Colors { get; set; } = new List<Color>();
    }
}

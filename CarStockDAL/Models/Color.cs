namespace CarStockDAL.Models
{
    public class Color
    {
        /// <summary>
        /// Идентификатор цвета автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название цвета автомобиля
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор связанной модели автомобиля
        /// </summary>
        public int CarModelId { get; set; } //required FK

        /// <summary>
        /// Навигационное свойство к модели автомобиля
        /// </summary>
        public CarModel CarModel { get; set; }
    }
}

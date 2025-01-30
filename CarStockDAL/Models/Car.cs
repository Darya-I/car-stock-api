namespace CarStockDAL.Models
{
    public class Car
    {
        /// <summary>
        /// Идентификатор автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Количество автомобиля
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Доступность автомобиля
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Идентификатор связанной марки
        /// </summary>
        public int BrandId { get; set; } //required FK

        /// <summary>
        /// Навигационное свойство к марке автомобиля
        /// </summary>
        public Brand? Brand { get; set; }

        /// <summary>
        /// Идентификатор модели автомобиля
        /// </summary>
        public int CarModelId { get; set; }
        
        /// <summary>
        /// Навигационное свойство к модели автомобиля
        /// </summary>
        public CarModel? CarModel { get; set; }

        /// <summary>
        /// Идентификатор цвета автомобиля
        /// </summary>
        public int ColorId { get; set; }

        /// <summary>
        /// Навигационное свойство к цвету автомобиля
        /// </summary>
        public Color? Color { get; set; }                                
    }
}

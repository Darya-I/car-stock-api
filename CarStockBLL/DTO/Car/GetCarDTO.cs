namespace CarStockBLL.DTO.Car
{
    /// <summary>
    /// DTO автомобиля с названиями для представления
    /// </summary>
    public class GetCarDTO
    {
        /// <summary>
        /// Идентификатор автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Навигационное свойство к марке автомобиля
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Навигационное свойство к модели автомобиля
        /// </summary>
        public string CarModel { get; set; }

        /// <summary>
        /// Навигационное свойство к цвету автомобиля
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Количество автомобиля
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Доступность автомобиля
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
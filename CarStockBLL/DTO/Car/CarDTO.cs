namespace CarStockBLL.DTO.Car
{
    /// <summary>
    /// DTO автомобиля
    /// </summary>
    public class CarDTO
    {
        /// <summary>
        /// Идентификатор автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор марки автомобиля
        /// </summary>
        public int BrandId { get; set; }

        /// <summary>
        /// Идентификатор модели автомобиля
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// Идентификатор цвета автомобиля
        /// </summary>
        public int ColorId { get; set; }

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

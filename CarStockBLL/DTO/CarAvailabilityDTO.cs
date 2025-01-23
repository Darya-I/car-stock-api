namespace CarStockBLL.DTO
{
    /// <summary>
    /// DTO обновления наличия автомобиля
    /// </summary>
    public class CarAvailabilityDTO
    {
        /// <summary>
        /// Идентификатор автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Доступность автомобиля
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}

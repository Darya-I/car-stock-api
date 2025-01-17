namespace CarStockMAP.DTO.Car
{
    /// <summary>
    /// DTO обновления наличия автомобиля
    /// </summary>
    public class CarAvailabilityUpdateDTO
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

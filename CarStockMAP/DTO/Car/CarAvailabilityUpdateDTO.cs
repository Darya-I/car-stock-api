namespace CarStockMAP.DTO.Car
{
    /// <summary>
    /// DTO обновления наличия автомобиля
    /// </summary>
    public class CarAvailabilityUpdateDTO
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
    }
}

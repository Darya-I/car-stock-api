namespace CarStockBLL.DTO.Car
{
    /// <summary>
    /// DTO автомобиля с названиями для представления
    /// </summary>
    public class GetCarDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string CarModel { get; set; }
        public string Color { get; set; }
        public int Amount { get; set; }
        public bool IsAvailable { get; set; }
    }
}
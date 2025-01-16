namespace CarStockMAP.DTO.Car
{
    /// <summary>
    /// DTO обновленного автомобиля
    /// </summary>
    public class CarUpdateDTO
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int CarModelId { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
        public bool IsAvailable { get; set; }
    }
}

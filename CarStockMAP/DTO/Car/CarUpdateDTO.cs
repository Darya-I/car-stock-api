namespace CarStockMAP.DTO.Car
{
    public class CarUpdateDTO
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int CarModelId { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
        public bool IsAvaible { get; set; }
    }
}

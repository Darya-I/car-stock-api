namespace CarStockDAL.Models
{
    public class Car
    {
        public int Id { get; set; }

        public int BrandId { get; set; } //required FK
        public int ModelId { get; set; } //required FK
        public int ColorId { get; set; } //required FK

        public Brand Brand { get; set; }
        public Color Color { get; set; }
        public CarModel Model { get; set; }
    }
}

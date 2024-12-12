namespace CarStockDAL.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public bool IsAvaible { get; set; }

        public int BrandId { get; set; } //required FK
        public Brand Brand { get; set; }

        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }

                                        
    }
}

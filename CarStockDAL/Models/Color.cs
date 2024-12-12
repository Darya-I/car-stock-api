namespace CarStockDAL.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CarModelId { get; set; } //required FK
        public CarModel CarModel { get; set; }
    }
}

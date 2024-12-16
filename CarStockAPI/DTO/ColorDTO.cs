namespace CarStockAPI.DTO
{
    public class ColorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CarModelId { get; set; }
        public CarModelDTO CarModel { get; set; }

    }
}

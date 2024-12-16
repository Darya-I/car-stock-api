namespace CarStockAPI.DTO
{
    public class CarModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int BrandId { get; set; }
        public BrandDTO Brand { get; set; }

        public ICollection<ColorDTO> Colors { get; set; } = new List<ColorDTO>();
    }
}

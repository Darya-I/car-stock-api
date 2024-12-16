namespace CarStockAPI.DTO
{
    public class BrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CarModelDTO> CarModels { get; set; } = new List<CarModelDTO>();
        public ICollection<CarDTO> Cars { get; set; } = new List<CarDTO>();
    }
}

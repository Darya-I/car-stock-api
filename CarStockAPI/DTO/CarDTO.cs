using System.Text.Json.Serialization;

namespace CarStockAPI.DTO
{
    public class CarDTO
    {
                    // навигация указывается для маппинга
        public int Id { get; set; }
        public int Amount { get; set; }
        public bool IsAvaible { get; set; }

        public int BrandId { get; set; }
        public BrandDTO Brand { get; set; }
        public int CarModelId { get; set; }
        
        public CarModelDTO CarModel { get; set; }
        public int ColorId { get; set; }
        public ColorDTO Color { get; set; }
    }
}
